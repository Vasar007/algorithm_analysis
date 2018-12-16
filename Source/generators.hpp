#pragma once

#include <algorithm>
#include <cassert>
#include <iterator>
#include <limits>
#include <vector>

#include "graph.hpp"


namespace gen
{



[[nodiscard]] vv::edge<int, long long> create_edge(const int exclude_border)
{
    int src = utils::random_number(0, exclude_border);
    int dest = utils::random_number(0, exclude_border);

    // Check if we have self-connected edge.
    while (src == dest)
    {
        src = utils::random_number(0, exclude_border);
        dest = utils::random_number(0, exclude_border);
    }

    return { src, dest, utils::random_number<long long>(0, 10'000) };
}

template <class Type, class WeightT = int>
void check_and_fix(vv::graph<Type, WeightT>& g)
{
    if (!g.is_correct())
    {
        g.silent_fix();
    }
}

// A function to generate random graph.
[[nodiscard]] vv::graph<int, long long>
generate_rand_graph(const int vertices_number)
{
    assert(vertices_number > 1);

    const int edges_number = 3 * vertices_number;
    std::vector<vv::edge<int, long long>> edges;

    int i = 0;
    while (i < edges_number)
    {
        // Because we number vertices with 0, need to exclude upper bound value.
        auto new_edge = create_edge(vertices_number - 1);

        for (const auto& e : edges)
        {
            if (new_edge == e || new_edge == reversed(e))
            {
                continue;
            }
        }
        edges.emplace_back(std::move(new_edge));
        ++i;
    }

    vv::graph<int, long long> g(edges);
    check_and_fix(g);
    return g;
}


[[nodiscard]] vv::graph<int, long long>
generate_tricky_case(const int vertices_number = 30)
{
    assert(vertices_number > 1);

    // Set some big value for edge weights.
    constexpr int W = static_cast<int>(8e8);

    // Generate vector of graph edges. This is complex case which can cause exponential complexity
    // if implementation of Levit's algorithm would use deque instead of two queues.
    std::vector<vv::edge<int, long long>> edges;
    for (int i = 0; i < vertices_number; ++i)
    {
        edges.emplace_back(i, i + 1, 0);
    }

    edges.emplace_back(0, vertices_number, W);
    int n = vertices_number;

    for (int i = 0; i < vertices_number; ++i)
    {
        edges.emplace_back(n, n + 2, W >> i);
        edges.emplace_back(n, n + 1, 0);
        edges.emplace_back(n + 1, n + 2, 0);
        n += 2;
    }
    ++n;

    // Shuffle result edges.
    std::shuffle(std::begin(edges), std::end(edges), utils::RANDOM_ENGINE);

    // Help hash map to reserve proper number of buckets.
    const std::size_t N = vertices_number * 3 + 1;
    // Define bilateral graph property.
    constexpr bool bilateral = true;

    // Construct graph.
    vv::graph<int, long long> g(edges, bilateral, N);
    check_and_fix(g);
    return g;
}

} // namespace gen
