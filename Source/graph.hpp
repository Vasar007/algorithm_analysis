// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#pragma once

#include <algorithm>
#include <deque>
#include <limits>
#include <numeric>
#include <ostream>
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
constexpr edge<Type, WeightT> reversed(edge<Type, WeightT> e) noexcept
{
    e.reverse();
    return e;
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


    graph(const std::vector<edge<value_type, weight_type>>& edges, const bool bilateral = false,
          const size_type N = 100)
    : bilateral(bilateral)
    {
        // If N specified, we can reserve proper number of buckets in hash map and minimize hash
        // collisions.
        _adjacency_list.reserve(N);

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


    std::pair<size_type, container_size_type> fullsize() const
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

        // Insert at the end.
        _adjacency_list[src].emplace_back(dest, weight);
        // If graph is bilateral, we add reverse edge.
        if (bilateral)
        {
            _adjacency_list[dest].emplace_back(src, weight);
        }
    }


    bool remove(const value_type& v)
    {
        if (const auto search = _adjacency_list.find(v); search != std::end(_adjacency_list))
        {
            _adjacency_list.erase(search);
            return true;
        }
        return false;
    }


    bool remove(const edge<value_type, weight_type>& e)
    {
        if (_remove_edge(e))
        {
            if (bilateral)
            {
                return _remove_edge(reversed(e));
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


    // Remove incorrect verticies in adjency list.
    bool fix()
    {
        bool is_changed = false;
        for (auto it_adj_list = std::begin(_adjacency_list);
             it_adj_list != std::end(_adjacency_list);)
        {
            auto& list = it_adj_list->second;
            for (auto it_cont = std::begin(list); it_cont != std::end(list);)
            {
                if (const auto search = _adjacency_list.find(it_cont->first);
                    search == std::end(_adjacency_list))
                {
                    it_cont = list.erase(it_cont);
                    is_changed = true;
                }
                else
                {
                    ++it_cont;
                }
            }

            if (list.empty())
            {
                it_adj_list = _adjacency_list.erase(it_adj_list);
            }
            else
            {
                ++it_adj_list;
            }
        }
        return is_changed;
    }


    // Reassign incorrect verticies in adjency list.
    bool silent_fix()
    {
        bool is_changed = false;
        for (auto& [vertex, list] : _adjacency_list)
        {
            for (auto& [dest, weight] : list)
            {
                if (const auto search = _adjacency_list.find(dest);
                    search == std::end(_adjacency_list))
                {
                    dest = utils::take_accidentally(_adjacency_list).first;
                    is_changed = true;
                }
            }
        }
        return is_changed;
    }

private:
    // Construct a unordered_map of vectors of pairs to represent an adjacency list.
    data_container _adjacency_list;

    bool bilateral;


    bool _remove_edge(const edge<value_type, weight_type>& e)
    {
        if (const auto search = _adjacency_list.find(e.src); search != std::end(_adjacency_list))
        {
            const auto& list_container = search->second;
            const auto it = std::find_if(
                std::begin(list_container), std::end(list_container),
                [&e](const pair& p)
                {
                    return p.first == e.dest && p.second == e.weight;
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
std::ostream& operator<<(std::ostream& os, const graph<Type, WeightT>& g)
{
    const auto [vertices_size, edges_size] = g.fullsize();
    os << "Graph size: [" << vertices_size << 'x' << edges_size << "]\n";
    for (const auto& [vertex, list] : g.data())
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


// Levit's algorithm implementation.
template <class Type, class WeightT = int>
std::unordered_map<Type, WeightT> levit_algorithm(const graph<Type, WeightT>& g, const Type& s)
{
    static_assert(std::is_integral_v<Type>,
                  "Vertex elements type has to be integral for Levit's algorithm!");

    std::unordered_set<Type> m0;
    std::deque<Type> m1{ s };
    std::deque<Type> m1_;
    std::unordered_set<Type> m2;

    const std::size_t N = g.data().size();
    constexpr WeightT INF = std::numeric_limits<WeightT>::max();

    std::unordered_map<Type, WeightT> distances;
    distances.reserve(N);

    m0.reserve(N);
    m2.reserve(N);

    for (const auto& [vertex, list] : g.data())
    {
        if (vertex != s)
        {
            distances[vertex] = INF;
            m2.insert(vertex);
        }
        else
        {
            distances[vertex] = 0;
        }
    }

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

        for (const auto& [v, weight] : g.data().at(u))
        {
            const WeightT new_weight = distances.at(u) + weight;
            if (const auto search_m2 = m2.find(v); search_m2 != std::end(m2))
            {
                m1.push_back(v);
                m2.erase(search_m2);
                distances.at(v) = std::min(distances.at(v), new_weight);
            }
            else if (std::find(std::begin(m1), std::end(m1), v) != std::end(m1) ||
                     std::find(std::begin(m1_), std::end(m1_), v) != std::end(m1_))
            {
                distances.at(v) = std::min(distances.at(v), new_weight);
            }
            else if (const auto search_m0 = m0.find(v);
                     search_m0 != std::end(m0) && distances.at(v) > new_weight)
            {
                m1_.push_back(v);
                m0.erase(search_m0);
                distances.at(v) = new_weight;
            }
        }
        m0.insert(u);
    }

    return distances;
}

} // namespace vv
