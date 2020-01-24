// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#pragma once

#include <algorithm>
#include <deque>
#include <fstream>
#include <limits>
#include <numeric>
#include <ostream>
#include <sstream>
#include <string>
#include <string_view>
#include <type_traits>
#include <unordered_map>
#include <unordered_set>
#include <utility>
#include <vector>

#include "utils.hpp"


namespace vv
{

// Data structure to store graph edges.
template <class Type, class WeightT = int>
struct edge
{
    Type src;
    Type dest;
    WeightT weight;

    constexpr edge(const Type& src, const Type& dest, const WeightT& weight)
    : src(src)
    , dest(dest)
    , weight(weight)
    {
    }

    constexpr bool equal(const edge& other) const noexcept
    {
        return *this == other && weight == other.weight;
    }

    constexpr void reverse() noexcept
    {
        std::swap(dest, src);
    }
};

template <class Type, class WeightT = int>
constexpr bool operator==(const edge<Type, WeightT>& lhs, const edge<Type, WeightT>& rhs) noexcept
{
    return lhs.src == rhs.src && lhs.dest == rhs.dest;
}

template <class Type, class WeightT = int>
constexpr edge<Type, WeightT> reversed(edge<Type, WeightT> edge_value) noexcept
{
    edge_value.reverse();
    return edge_value;
}


// Graph implementation with adjacency list.
template <class Type, class WeightT = int>
class graph
{
public:
    // Alias type names for convenient further using and development.
    using value_type                     = Type;
    using size_type                      = std::size_t;
    using difference_type                = std::ptrdiff_t;
    using weight_type                    = WeightT;

    using pair                           = std::pair<value_type, weight_type>;
    using container                      = std::vector<pair>;
    using container_reference            = container&;
    using const_container_reference      = const container&;

    using data_container                 = std::unordered_map<value_type, container>;
    using data_container_reference       = data_container&;
    using const_data_container_reference = const data_container&;
    using container_size_type            = typename data_container::size_type;
    using container_value_type           = typename data_container::value_type;

    using pointer                        = value_type*;
    using const_pointer                  = const value_type*;

    using reference                      = value_type&;
    using const_reference                = const value_type&;


    explicit graph(const bool bilateral = false)
    : _bilateral(bilateral)
    {
    }

    explicit graph(const std::vector<edge<value_type, weight_type>>& edges,
                   const bool bilateral = false, const size_type vertices_number = 100)
    : _bilateral(bilateral)
    {
        // If vertices_number specified, we can reserve proper number of buckets in hash map and
        // minimize hash collisions.
        _adjacency_list.reserve(vertices_number);

        // Add edges to the directed graph.
        for (const auto& e : edges)
        {
            insert(e);
        }
    }

    data_container_reference data() noexcept
    {
        return _adjacency_list;
    }

    const_data_container_reference data() const noexcept
    {
        return _adjacency_list;
    }

    container_size_type size() const noexcept
    {
        return _adjacency_list.size();
    }

    std::pair<size_type, container_size_type> full_size() const
    {
        // Calculate sizes of set V and set E.
        const container_size_type edges_size = std::accumulate(
            std::begin(_adjacency_list), std::end(_adjacency_list),
            static_cast<container_size_type>(0),
            [](const container_size_type& init, const container_value_type& pair)
            {
                 return init + pair.second.size();
            }
        );
        return { _adjacency_list.size(), edges_size };
    }

    void insert(const edge<value_type, weight_type>& new_edge)
    {
        const value_type& src = new_edge.src;
        const value_type& dest = new_edge.dest;
        const weight_type& weight = new_edge.weight;

        if (const auto search = _adjacency_list.find(src); search == std::end(_adjacency_list))
        {
            _adjacency_list.emplace(src, container{});
        }

        // Insert at the end.
        _adjacency_list[src].emplace_back(dest, weight);

        if (const auto search = _adjacency_list.find(dest); search == std::end(_adjacency_list))
        {
            _adjacency_list.emplace(dest, container{});
        }

        // If graph is bilateral, we add reverse edge.
        if (_bilateral)
        {
            _adjacency_list[dest].emplace_back(src, weight);
        }
    }

    bool remove(const value_type& vertex)
    {
        if (const auto search = _adjacency_list.find(vertex); search != std::end(_adjacency_list))
        {
            _adjacency_list.erase(search);
            return true;
        }
        return false;
    }

    bool remove(const edge<value_type, weight_type>& edge_value)
    {
        if (_remove_edge(edge_value))
        {
            if (_bilateral)
            {
                return _remove_edge(reversed(edge_value));
            }
            return true;
        }
        return false;
    }

    bool is_correct() const
    {
        for (const auto& [src, list] : _adjacency_list)
        {
            for (const auto& [dest, weight] : list)
            {
                if (const auto search = _adjacency_list.find(dest);
                    search == std::end(_adjacency_list))
                {
                    return false;
                }
            }
        }
        return true;
    }

    // Remove incorrect vertices in adjacency list.
    bool fix()
    {
        bool was_changed = false;
        for (auto it_adj_list = std::begin(_adjacency_list);
             it_adj_list != std::end(_adjacency_list); ++it_adj_list)
        {
            auto& list = it_adj_list->second;
            for (auto it_cont = std::begin(list); it_cont != std::end(list);)
            {
                if (const auto search = _adjacency_list.find(it_cont->first);
                    search == std::end(_adjacency_list))
                {
                    it_cont = list.erase(it_cont);
                    was_changed = true;
                }
                else
                {
                    ++it_cont;
                }
            }
        }

        assert(is_correct());
        return was_changed;
    }

    // Reassign incorrect vertices in adjacency list.
    bool silent_fix()
    {
        bool was_changed = false;
        for (auto& [vertex, list] : _adjacency_list)
        {
            for (auto& [dest, weight] : list)
            {
                if (const auto search = _adjacency_list.find(dest);
                    search == std::end(_adjacency_list))
                {
                    // Avoid self-circled vertices.
                    do
                    {
                        dest = utils::take_accidentally(_adjacency_list).first;
                    }
                    while (dest == vertex);

                    was_changed = true;
                }
            }
        }

        assert(is_correct());
        return was_changed;
    }

private:
    // Construct a unordered_map of vectors of pairs to represent an adjacency list.
    data_container _adjacency_list;

    bool _bilateral;


    bool _remove_edge(const edge<value_type, weight_type>& edge_value)
    {
        if (const auto search = _adjacency_list.find(edge_value.src);
            search != std::end(_adjacency_list))
        {
            const auto& list_container = search->second;
            const auto it = std::find_if(
                std::begin(list_container), std::end(list_container),
                [&edge_value](const pair& p)
                {
                    return p.first == edge_value.dest && p.second == edge_value.weight;
                }
            );
            if (it != std::end(list_container))
            {
                list_container.erase(it);
                return true;
            }
        }

        return false;
    }
};

template <class Type, class WeightT = int>
std::ostream& operator<<(std::ostream& os, const graph<Type, WeightT>& graph_instance)
{
    const auto [vertices_size, edges_size] = graph_instance.full_size();
    os << "Graph size: [" << vertices_size << 'x' << edges_size << "]\n";
    for (const auto& [vertex, list] : graph_instance.data())
    {
        // Print current vertex number.
        os << vertex << " => ";

        // Print all neighboring vertices of vertex i.
        for (const auto& v : list)
        {
            os << v.first << ' ' << v.second << " | ";
        }
        os << '\n';
    }
    return os;
}

int get_full_graph_edges_number(const int vertices_number)
{
    return vertices_number * (vertices_number - 1) / 2;
}


graph<int, long long> read_csv_and_add_weights(const std::string_view path)
{
    if (std::ifstream infile(path.data()); infile.is_open())
    {
        std::string line;
        std::vector<vv::edge<int, long long>> edges;
        std::size_t vertices_number = 0;

        while (std::getline(infile, line))
        {
            std::istringstream iss(line);
            
            int source = -1;
            int dest;
            while (iss >> dest)
            {
                if (source == -1)
                {
                    source = dest;
                    ++vertices_number;
                    continue;
                }

                edges.emplace_back(
                    source, dest, utils::random_number<long long>(0, utils::UPPER_BORDER)
                );
            }
        }

        // Define bilateral graph property.
        constexpr bool bilateral = false;

        graph<int, long long> graph_instance(edges, bilateral, vertices_number);
        assert(graph_instance.is_correct());

        return graph_instance;
    }
    
    std::cout << "Could not open file!\n";
    return graph<int, long long>{};
}

// Levit's algorithm implementation with two data structures for M1.
template <class Type, class WeightT = int>
std::unordered_map<Type, WeightT> levit_algorithm(const graph<Type, WeightT>& graph_instance, 
                                                  const Type& start_vertex)
{
    static_assert(std::is_integral_v<Type>,
                  "Vertex elements type has to be integral for Levit's algorithm!");

    std::unordered_set<Type> m0;
    std::deque<Type> m1{ start_vertex };
    std::deque<Type> m1_;
    std::unordered_set<Type> m2;

    const std::size_t N = graph_instance.data().size();
    constexpr WeightT INF = std::numeric_limits<WeightT>::max();

    m0.reserve(N);
    m2.reserve(N);

    std::unordered_map<Type, WeightT> distances;
    distances.reserve(N);

    for (const auto& [vertex, list] : graph_instance.data())
    {
        if (vertex != start_vertex)
        {
            distances[vertex] = INF;
            m2.insert(vertex);
        }
        else
        {
            distances[vertex] = 0;
        }
    }

    int counter = 0;
    while (!m1.empty() || !m1_.empty())
    {
        Type u;
        if (m1_.empty())
        {
            u = std::move(m1.front());
            m1.pop_front();
        }
        else
        {
            u = std::move(m1_.front());
            m1_.pop_front();
        }

        for (const auto& [v, weight] : graph_instance.data().at(u))
        {
            const WeightT new_weight = distances.at(u) + weight;
            if (const auto search_m2 = m2.find(v); search_m2 != std::end(m2))
            {
                m1.push_back(v);
                m2.erase(search_m2);
                distances.at(v) = std::min(distances.at(v), new_weight);
                ++counter;
            }
            else if (std::find(std::begin(m1), std::end(m1), v) != std::end(m1) ||
                     std::find(std::begin(m1_), std::end(m1_), v) != std::end(m1_))
            {
                distances.at(v) = std::min(distances.at(v), new_weight);
                ++counter;
            }
            else if (const auto search_m0 = m0.find(v);
                     search_m0 != std::end(m0) && distances.at(v) > new_weight)
            {
                m1_.push_back(v);
                m0.erase(search_m0);
                distances.at(v) = new_weight;
                ++counter;
            }
        }
        m0.insert(u);
    }

    std::cout << "Operations: " << counter << '\n';

    return distances;
}

// Levit's algorithm implementation with one data structure for M1.
template <class Type, class WeightT = int>
std::vector<WeightT> levit_algorithm_2(const graph<Type, WeightT>& graph_instance,
                                       const Type& start_vertex)
{
    static_assert(std::is_integral_v<Type>,
        "Vertex elements type has to be integral for Levit's algorthm!");

    const std::size_t N = graph_instance.data().size();
    constexpr WeightT INF = std::numeric_limits<WeightT>::max();

    std::vector<WeightT> distances(N, INF);
    distances.at(start_vertex) = 0;

    std::vector<Type> id(N);
    std::deque<Type> q;
    q.push_back(start_vertex);
    std::vector<Type> p(N, -1);

    int counter = 0;
    while (!q.empty())
    {
        const Type v = q.front();
        q.pop_front();
        id[v] = 1;
        for (std::size_t i = 0; i < graph_instance.data().at(v).size(); ++i)
        {
            const Type to = graph_instance.data().at(v).at(i).first;
            const Type len = graph_instance.data().at(v).at(i).second;
            if (distances[to] > distances[v] + len)
            {
                distances[to] = distances[v] + len;
                if (id[to] == 0)
                {
                    q.push_back(to);
                    ++counter;
                }
                else if (id[to] == 1)
                {
                    q.push_back(to);
                    ++counter;
                }
                p[to] = v;
                id[to] = 1;
            }
        }
    }

    std::cout << "Operations: " << counter << '\n';

    return distances;
}

// Levit's algorithm implementation with counter.
template <class Type, class WeightT = int>
std::pair<std::unordered_map<Type, WeightT>, int> levit_algorithm_with_counter(
    const graph<Type, WeightT>& graph_instance, const Type& start_vertex)
{
    static_assert(std::is_integral_v<Type>,
                  "Vertex elements type has to be integral for Levit's algorithm!");

    std::unordered_set<Type> m0;
    std::deque<Type> m1{ start_vertex };
    std::deque<Type> m1_;
    std::unordered_set<Type> m2;

    const std::size_t N = graph_instance.data().size();
    constexpr WeightT INF = std::numeric_limits<WeightT>::max();

    std::unordered_map<Type, WeightT> distances;
    distances.reserve(N);

    m0.reserve(N);
    m2.reserve(N);

    for (const auto& [vertex, list] : graph_instance.data())
    {
        if (vertex != start_vertex)
        {
            distances[vertex] = INF;
            m2.insert(vertex);
        }
        else
        {
            distances[vertex] = 0;
        }
    }

    int counter = 0;
    while (!m1.empty() || !m1_.empty())
    {
        Type u;
        if (m1_.empty())
        {
            u = std::move(m1.front());
            m1.pop_front();
        }
        else
        {
            u = std::move(m1_.front());
            m1_.pop_front();
        }

        for (const auto& [v, weight] : graph_instance.data().at(u))
        {
            const WeightT new_weight = distances.at(u) + weight;
            if (const auto search_m2 = m2.find(v); search_m2 != std::end(m2))
            {
                m1.push_back(v);
                m2.erase(search_m2);
                distances.at(v) = std::min(distances.at(v), new_weight);
                ++counter;
            }
            else if (std::find(std::begin(m1), std::end(m1), v) != std::end(m1) ||
                     std::find(std::begin(m1_), std::end(m1_), v) != std::end(m1_))
            {
                distances.at(v) = std::min(distances.at(v), new_weight);
                ++counter;
            }
            else if (const auto search_m0 = m0.find(v);
                     search_m0 != std::end(m0) && distances.at(v) > new_weight)
            {
                m1_.push_back(v);
                m0.erase(search_m0);
                distances.at(v) = new_weight;
                ++counter;
            }
        }
        m0.insert(u);
    }

    return { distances, counter };
}

// Ford-Bellman's algorithm implementation with counter.
template <class Type, class WeightT = int>
std::pair<std::unordered_map<Type, WeightT>, int> ford_bellman_algorithm_with_counter(
    const graph<Type, WeightT>& graph_instance, const Type& s)
{
    static_assert(std::is_integral_v<Type>,
                  "Vertex elements type has to be integral for Levit's algorithm!");

    const std::size_t N = graph_instance.data().size();
    constexpr WeightT INF = std::numeric_limits<WeightT>::max();

    std::unordered_map<Type, WeightT> distances;
    distances.reserve(N);

    for (const auto& [vertex, list] : graph_instance.data())
    {
        if (vertex != s)
        {
            distances[vertex] = INF;
        }
        else
        {
            distances[vertex] = 0;
        }
    }

    int counter = 0;
    bool stop = false;
    for (std::size_t k = 0; k < N - 1 && !stop; ++k)
    {
        stop = true;
        for (const auto& [u, list] : graph_instance.data())
        {
            for (const auto& [v, weight] : list)
            {
                if (distances.at(u) != INF && distances.at(u) + weight < distances.at(v))
                {
                    distances.at(v) = distances.at(u) + weight;
                    stop = false;
                    ++counter;
                }
            }
        }
    }

    return { distances, counter };
}

} // namespace vv
