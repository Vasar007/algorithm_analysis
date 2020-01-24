// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#pragma once

#include <algorithm>
#include <cassert>
#include <iterator>
#include <limits>
#include <vector>
#include <utility>

#include "graph.hpp"
#include "utils.hpp"


namespace gen
{

[[nodiscard]] vv::edge<int, long long> create_edge(const int upper_border)
{
    int src = utils::random_number(0, upper_border);
    int dest = utils::random_number(0, upper_border);

    // Check if we have self-connected edge.
    while (src == dest)
    {
        src = utils::random_number(0, upper_border);
        dest = utils::random_number(0, upper_border);
    }

    return { src, dest, utils::random_number<long long>(0, utils::UPPER_BORDER) };
}

////////////////////////////////////////////////////////////////////////////////////////////////////

template <class Type, class WeightT = int>
void check_and_fix(vv::graph<Type, WeightT>& graph_instance)
{
    if (!graph_instance.is_correct())
    {
        graph_instance.silent_fix();
    }
    assert(graph_instance.is_correct());
}

////////////////////////////////////////////////////////////////////////////////////////////////////

// A function to generate random graph.
[[nodiscard]] vv::graph<int, long long>
generate_rand_graph(const int vertices_number, const int edges_number)
{
    // Reject simple graph.
    assert(vertices_number > 1);
    assert(edges_number > 0);

    std::vector<vv::edge<int, long long>> edges;
    edges.reserve(edges_number);

    int i = 0;
    while (i < edges_number)
    {
        bool contains = false;

        // Because we number vertices with 0, need to exclude upper bound value.
        auto new_edge = create_edge(vertices_number - 1);

        for (const auto& e : edges)
        {
            if (new_edge == e || new_edge == reversed(e))
            {
                contains = true;
                break;
            }
        }

        if (!contains)
        {
            edges.emplace_back(new_edge);
            ++i;
        }
    }

    // Define bilateral graph property.
    constexpr bool bilateral = false;

    vv::graph<int, long long> graph_instance(edges, bilateral, vertices_number);
    check_and_fix(graph_instance);
    return graph_instance;
}

////////////////////////////////////////////////////////////////////////////////////////////////////

// A function to generate random graph.
[[nodiscard]] vv::graph<int, long long>
generate_full_graph(const int vertices_number)
{
    // Reject simple graph.
    assert(vertices_number > 1);

    const int edges_number = vv::get_full_graph_edges_number(vertices_number);

    std::vector<vv::edge<int, long long>> edges;
    edges.reserve(edges_number);

    for (int src = 0; src < vertices_number; ++src)
    {
        for (int dest = src + 1; dest < vertices_number; ++dest)
        {
            edges.emplace_back(
                src, dest, utils::random_number<long long>(0, utils::UPPER_BORDER)
            );
        }
    }

    // TODO: check triangle inequality and fix graph.

    // Define bilateral graph property.
    constexpr bool bilateral = true;

    vv::graph<int, long long> graph_instance(edges, bilateral, vertices_number);
    assert(graph_instance.is_correct());
    return graph_instance;
}

[[nodiscard]] vv::graph<int, long long>
generate_tricky_case(const std::size_t vertices_number = 30)
{
    assert(vertices_number > 1);

    // Set some big value for edge weights.
    constexpr int big_value = static_cast<int>(8e8);

    // Generate vector of graph edges. This is complex case which can cause exponential complexity
    // if implementation of Levit's algorithm would use deque instead of two queues.
    std::vector<vv::edge<int, long long>> edges;
    edges.reserve(vertices_number * 4 + 1);
    for (int i = 0; i < vertices_number; ++i)
    {
        edges.emplace_back(i, i + 1, 0);
    }

    edges.emplace_back(0, vertices_number, big_value);
    int n = static_cast<int>(vertices_number);

    for (int i = 0; i < vertices_number; ++i)
    {
        // Process shift count overflow.
        if (i >= 30)
        {
            edges.emplace_back(n, n + 2, 1);
        }
        else
        {
            edges.emplace_back(n, n + 2, big_value >> i);
        }
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
    vv::graph<int, long long> graph_instance(edges, bilateral, N);
    check_and_fix(graph_instance);
    return graph_instance;
}

////////////////////////////////////////////////////////////////////////////////////////////////////

[[nodiscard]] vv::graph<int, long long>
generate_tricky_case_article(const int n_value = 3)
{
    assert(0 < n_value && n_value < 31);

    const int vertices_number = 2 * n_value + 1;
    const int edges_number = 3 * n_value + 2 * (n_value - 1);

    // Set some big value for edge weights.
    const auto big_value = static_cast<long long>(std::pow(2, n_value + 1) + 1);

    // Generate vector of graph edges. This is complex case which can cause exponential complexity
    // if implementation of Levit's algorithm would use deque instead of two queues.
    std::vector<vv::edge<int, long long>> edges;
    edges.reserve(edges_number);
    for (int i = 0; i < n_value; ++i)
    {
        const int i_n = i * 2;
        edges.emplace_back(i_n, i_n + 1, std::pow(2, n_value - i - 1));
        edges.emplace_back(i_n + 1, i_n + 2, 0);
        edges.emplace_back(i_n, i_n + 2, std::pow(2, n_value - i));

        if (i > 0)
        {
            edges.emplace_back(0, i_n + 1, big_value);
            edges.emplace_back(0, i_n + 2, big_value);
        }
    }

    // Shuffle result edges.
    std::shuffle(std::begin(edges), std::end(edges), utils::RANDOM_ENGINE);

    // Define bilateral graph property.
    constexpr bool bilateral = false;

    // Construct graph.
    vv::graph<int, long long> graph_instance(edges, bilateral, vertices_number);
    check_and_fix(graph_instance);
    return graph_instance;
}

} // namespace gen
