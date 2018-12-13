// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#pragma once

#include <algorithm>
#include <deque>
#include <ostream>
#include <type_traits>
#include <unordered_set>
#include <unordered_map>
#include <utility>
#include <vector>


namespace vv
{

// Data structure to store graph edges.
template <class Type, class WeightT = int>
struct edge
{
    Type src;
    Type dest;
    WeightT weight;

    edge(const Type src, const Type dest, const WeightT weight)
    : src(src)
    , dest(dest)
    , weight(weight)
    {
    }
};

template <class Type, class WeightT = int>
class graph
{
public:
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

    using pointer                        = value_type*;
    using const_pointer                  = const value_type*;

    using reference                      = value_type&;
    using const_reference                = const value_type&;

    // Graph Constructor.
    graph(const std::vector<edge<value_type, WeightT>>& edges, const size_type N = 1)
    {
        _adjacency_list.reserve(N);

        // Add edges to the directed graph.
        for (const auto& edge: edges)
        {
            const value_type src = edge.src;
            const value_type dest = edge.dest;
            const weight_type weight = edge.weight;

            // Insert at the end.
            _adjacency_list[src].emplace_back(dest, weight);
            _adjacency_list[dest].emplace_back(src, weight);
        }
    }

    const data_container data() const noexcept
    {
        return _adjacency_list;
    }

    std::vector<WeightT> levit_algorithm(const value_type s) const
    {
        static_assert(std::is_integral_v<value_type>,
                      "Vertex elements type has to be integral for Levit's algorthm!");

        std::unordered_set<value_type> m0;
        std::deque<value_type> m1, m1_;
        std::unordered_set<value_type> m2;

        const std::size_t N = _adjacency_list.size();
        constexpr weight_type INF = std::numeric_limits<WeightT>::max();

        std::vector<weight_type> distances(N, INF);
        distances.at(s) = 0;

        m0.reserve(N);
        m2.reserve(N);

        m1.push_back(s);

        for (const auto& [vertex, list] : _adjacency_list)
        {
            if (vertex != s)
            {
                m2.insert(vertex);
            }
        }

        int counter = 0;
        while (!m1.empty() || !m1_.empty())
        {
            value_type u;
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

            for (const auto& [v, weight] : _adjacency_list.at(u))
            {
                const weight_type new_weight = distances.at(u) + weight;
                if (const auto search_m2 = m2.find(v); search_m2 != m2.end())
                {
                    m1.push_back(v);
                    m2.erase(search_m2);
                    distances.at(v) = std::min(distances.at(v), new_weight);
                    ++counter;
                }
                else if (std::find(std::begin(m1), std::end(m1), v) != m1.end() ||
                         std::find(std::begin(m1_), std::end(m1_), v) != m1_.end())
                {
                    distances.at(v) = std::min(distances.at(v), new_weight);
                }
                else if (const auto search_m0 = m0.find(v);
                         search_m0 != m0.end() && distances.at(v) > new_weight)
                {
                    m1_.push_back(v);
                    m0.erase(search_m0);
                    distances.at(v) = new_weight;
                    ++counter;
                }

                m0.insert(u);
            }
        }

        std::cout << "Couner = " << counter << '\n';

        return distances;
    }

private:
    // Construct a unordered_map of vectors of pairs to represent an adjacency list.
    data_container _adjacency_list;
};


template <class Type, class WeightT = int>
std::ostream& operator<<(std::ostream& os, const graph<Type, WeightT>& g)
{
    for (const auto& [vertex, list] : g.data())
    {
        // print current vertex number
        os << vertex << " => ";

        // print all neighboring vertices of vertex i
        for (const auto& v : list)
        {
            os << v.first << ' ' << v.second << " | ";
        }
        os << '\n';
    }
    return os;
}

} // namespace vv
