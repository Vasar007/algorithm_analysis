// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#pragma once

#include <algorithm>
#include <array>
#include <cassert>
#include <chrono>
#include <cmath>
#include <iostream>
#include <random>
#include <utility>
#include <vector>

#include "generators.hpp"
#include "graph.hpp"
#include "utils.hpp"


namespace tests_with_time
{

namespace detail
{

using dmilliseconds = std::chrono::duration<double, std::milli>;

template <class Type, class WeightT = int>
dmilliseconds _make_test(const vv::graph<Type, WeightT>& graph_instance, const Type& start_vertex,
                         const bool verbose = true)
{
    // Print adjacency list representation of graph.
    if (verbose)
    {
        std::cout << graph_instance << '\n';
    }

    // Set start time.
    const auto start = std::chrono::steady_clock::now();
    // Call Levit's algorithm.
    const auto result = vv::levit_algorithm(graph_instance, start_vertex);
    // Stop timer and calculate result's time.
    const dmilliseconds elapsed = std::chrono::steady_clock::now() - start;

    // Print results.
    std::cout << "Calculation time: " << elapsed.count() << " ms\n";
    if (verbose)
    {
        std::cout << "Final vector of weights:\n";
        utils::println_pair(std::cout, result);
    }
    std::cout << "\n\n";

    return elapsed;
}

} // namespace detail

detail::dmilliseconds test_0(const int start_vertex_id = 3, const bool verbose = true)
{
    // Number of nodes in the graph. NOT hardcoded value, need to help hash map to reserve proper
    // number of buckets.
    constexpr std::size_t N = 6;

    assert(0 <= start_vertex_id && start_vertex_id < N);

    // Vector of graph edges.
    const std::vector<vv::edge<int, long long>> edges =
    {
        // (u, v, w) -> edge from u to v having weight w.
        { 0, 1, 6 }, { 1, 2, 7 }, { 2, 0, 5 }, { 2, 1, 4 },
        { 3, 2, 10 }, { 4, 5, 1 }, { 5, 4, 3 }
    };

    // Define bilateral graph property.
    constexpr bool bilateral = false;

    // Construct graph.
    vv::graph<int, long long> g(edges, bilateral, N);

    return detail::_make_test(g, start_vertex_id, verbose);
}

detail::dmilliseconds test_1(const int vertices_number = 30, const int s = 0,
                             const bool verbose = true)
{
    assert(0 <= s && s < vertices_number * 3 + 1);
    const auto g = gen::generate_tricky_case(vertices_number);
    return detail::_make_test(g, s, verbose);
}

detail::dmilliseconds test_2(const int vertices_number = 10, const bool verbose = true)
{
    const int edges_number = vv::get_full_graph_edges_number(vertices_number);
    const auto g = gen::generate_rand_graph(vertices_number, edges_number);
    return detail::_make_test(g, utils::take_accidentally(g.data()).first, verbose);
}

/// Test section.

void time_tests_series()
{
    std::cout << "Execute default test suit\n";
    test_0();

    constexpr int start_vertex_id = 0; // Start vertex.
    constexpr bool verbose = false; // Output result flag.
    // Create tests array.
    constexpr std::array vertices_number{ 10, 20, 40, 80, 160, 320, 640, 1280, 2560 };

    // Tricky case for Levit's algorithm which can cause exponential complexity if implementation
    // merges queues M1' and M1'' into one M1.
    std::cout << "Execute tricky test suit\n";
    std::vector<std::pair<double, double>> time_results;
    time_results.reserve(vertices_number.size());
    for (const auto& i : vertices_number)
    {
        const int size = 3 * i + 1;
        if (size >= vertices_number.back()) break;

        std::cout << "Size: " << size << '\n';
        const auto result = test_1(i, start_vertex_id, verbose).count();
        time_results.emplace_back(size, result);
    }
    utils::out_data("complex_case.txt", "1p", "def",
                    "Tricky case for algorithm analysis", "Number of vertex", "Completion time, ms",
                    time_results);

    // Random test cases.
    std::cout << "Execute random test suit\n";
    time_results.clear();
    for (const auto& i : vertices_number)
    {
        std::cout << "Size: " << i << '\n';
        const auto result = test_2(i, verbose).count();
        time_results.emplace_back(i, result);
    }
    utils::out_data("rand_tests.txt", "1p", "def",
                    "Random tests for algorithm analysis", "Number of vertex",
                    "Completion time, ms",
                    time_results);
}

void average_time_tests_series()
{
    constexpr int start_vertex_id = 0; // Start vertex.
    constexpr bool verbose = false; // Output result flag.

    constexpr int start_value = 80;
    constexpr int end_value = 320;
    constexpr int launches_number = 10;
    constexpr int step = 10;

    // Tricky case for Levit's algorithm which can cause exponential complexity if implementation
    // merges queues M1' and M1'' into one M1.
    std::cout << "Execute tricky test suit\n";
    std::vector<std::pair<double, double>> time_results;
    time_results.reserve(end_value / start_value);
    for (int i = start_value; i * 3 + 1 <= end_value; i += step)
    {
        const int size = i * 3 + 1; // Get such vertices number because of tricky generator.
        std::cout << "Size: " << size << '\n';
        double result = 0;
        for (int j = 1; j <= launches_number; ++j)
        {
            const auto launch_result = test_1(i, start_vertex_id, verbose).count();
            result += launch_result;
        }

        time_results.emplace_back(size, result / launches_number);
    }
    utils::out_data("levit_complex_case_average_series.txt", "1p", "def",
                    "Tricky case for algorithm analysis", "Number of vertex", "Completion time, ms",
                    time_results);

    // Random test cases.
    std::cout << "Execute random test suit\n";
    time_results.clear();
    for (int i = start_value; i <= end_value; i += step)
    {
        std::cout << "Size: " << i << '\n';
        double result = 0;
        for (int j = 1; j <= launches_number; ++j)
        {
            const auto launch_result = test_2(i, verbose).count();
            result += launch_result;
        }

        time_results.emplace_back(i, result / launches_number);
    }

    utils::out_data("rand_tests_average_series.txt", "1p", "def",
                    "Random tests for algorithm analysis", "Number of vertex", "Operations number",
                    time_results);
}

void average_time_tests_relative()
{
    constexpr int start_vertex_id = 0; // Start vertex.
    constexpr bool verbose = false; // Output result flag.
    // Create tests array.
    constexpr std::array vertices_number{ 10, 20, 40, 80, 160, 320, 640, 1280, 2560 };
    constexpr int launches_number = 10;

    // Tricky case for Levit's algorithm which can cause exponential complexity if implementation
    // merges queues M1' and M1'' into one M1.
    std::cout << "Execute tricky test suit\n";
    std::vector<std::pair<double, double>> time_results;
    time_results.reserve(vertices_number.size());
    double prev = 1;
    for (const auto& i : vertices_number)
    {
        const int size = i * 3 + 1;
        if (size >= vertices_number.back()) break;

        std::cout << "Size: " << size << '\n';
        double result = 0;
        for (int j = 1; j <= launches_number; ++j)
        {
            const auto launch_result = test_1(i, start_vertex_id, verbose).count();
            result += launch_result;
        }

        result /= launches_number;
        const double rel_result = result / prev;
        prev = result;
        time_results.emplace_back(size, rel_result);
    }
    utils::out_data("complex_case_average_series_rel.txt", "1p", "def",
                    "Tricky case for algorithm analysis, relative", "Number of vertex",
                    "Completion time, ms",
                    time_results);

    // Random test cases.
    std::cout << "Execute random test suit\n";
    time_results.clear();
    prev = 1;
    for (const auto& i : vertices_number)
    {
        std::cout << "Size: " << i << '\n';
        double result = 0;
        for (int j = 1; j <= launches_number; ++j)
        {
            const auto launch_result = test_2(i, verbose).count();
            result += launch_result;
        }

        result /= launches_number;
        const double rel_result = result / prev;
        prev = result;
        time_results.emplace_back(i, rel_result);
    }
    utils::out_data("rand_tests_average_series_rel.txt", "1p", "def",
                    "Random tests for algorithm analysis, relative", "Number of vertex",
                    "Completion time, ms",
                    time_results);
}

void create_theoretical_data()
{
    std::cout << "Create theoretical results\n";
    // Create tests array.
    constexpr std::array vertices_number{ 10, 20, 40, 80, 160, 320, 640, 1280, 2560 };
    constexpr std::array vertices_number2{ 31, 61, 121, 241, 481, 961, 1921 };

    const auto average_case = [](const int n) -> std::pair<double, double>
    {
        //return { n, 5.35413e-8 * std::pow(n, 2.00015798) + 0.0180708 }; // rand case
        return { n, 8.83783e-7 * std::pow(n, 1.99995349) - 0.0360029 }; // bad case
    };

    std::vector<std::pair<double, double>> results;
    results.reserve(vertices_number.size());
    for (const auto& i : vertices_number)
    {
        results.emplace_back(average_case(i));
    }
    utils::out_data("theory_data.txt", "1p", "def",
                    "Theoretical results", "Number of vertex", "Completion time, ms",
                    results);

    results.clear();
    for (int i = vertices_number2.front(); i <= vertices_number2.back(); i += 10)
    {
        results.emplace_back(average_case(i));
    }
    utils::out_data("theory_data2.txt", "1p", "def",
                    "Theoretical results", "Number of vertex", "Completion time, ms",
                    results);
}

} // namespace tests_with_time
