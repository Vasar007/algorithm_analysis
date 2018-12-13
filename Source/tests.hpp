// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#pragma once

#include <algorithm>
#include <chrono>
#include <iostream>
#include <iterator>
#include <random>
#include <vector>

#include "graph.hpp"
#include "utils.hpp"

#include "tests.hpp"


namespace tests
{

template <class Type, class WeightT = int>
void _create_graph_and_make_test(const std::vector<vv::edge<Type, WeightT>>& edges, const Type& s,
                                 const std::size_t N = 1)
{
    // Construct graph.
    vv::graph<Type, WeightT> g(edges, N);

    // Print adjacency list representation of graph.
    std::cout << g << '\n';

    const auto result = g.levit_algorithm(s);
    utils::println(std::cout, result);
    std::cout << "\n\n";
}

void test_1(const int s = 3)
{
    // Vector of graph edges.
    std::vector<vv::edge<int, long long>> edges =
    {
        // (x, y, w) -> edge from x to y having weight w.
        { 0, 1, 6 }, { 1, 2, 7 }, { 2, 0, 5 }, { 2, 1, 4 },
        { 3, 2, 10 }, { 4, 5, 1 }, { 5, 4, 3 }
    };

    // Number of nodes in the graph. NOT hardcoded value, need to help hash map to reserve proper
    // number of buckets.
    constexpr std::size_t N = 6;

    _create_graph_and_make_test(edges, s, N);
}

void test_2(const int s = 0)
{
    constexpr int M = 30;
    constexpr int W = static_cast<int>(8e8);

    // Generate vector of graph edges. This is complex case which can cause exponential complexity.
    std::vector<vv::edge<int, long long>> edges;
    for (int i = 0; i < M; ++i)
    {
        edges.emplace_back(i, i + 1, 1);
    }

    edges.emplace_back(0, M, W);
    int n = M;

    for (int i = 0; i < M; ++i)
    {
        edges.emplace_back(n, n + 2, W >> i);
        edges.emplace_back(n, n + 1, 1);
        edges.emplace_back(n + 1, n + 2, 1);
        n += 2;
    }
    ++n;

    // Obtain a time-based seed:
    const unsigned seed = std::chrono::system_clock::now().time_since_epoch().count();
    std::default_random_engine default_engine(seed);

    std::shuffle(std::begin(edges), std::end(edges), default_engine);

    // Number of nodes in the graph. NOT hardcoded value, need to help hash map to reserve proper
    // number of buckets.
    constexpr std::size_t N = 91;

    _create_graph_and_make_test(edges, s, N);
}

} // namespace tests
