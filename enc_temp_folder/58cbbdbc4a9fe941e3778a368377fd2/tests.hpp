// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#pragma once

#include <algorithm>
#include <array>
#include <cassert>
#include <chrono>
#include <cmath>
#include <iostream>
#include <iterator>
#include <random>
#include <utility>
#include <vector>

#include "generators.hpp"
#include "graph.hpp"
#include "utils.hpp"


namespace tests
{

namespace detail
{

template <class Type, class WeightT = int>
std::chrono::duration<double> _make_test(
    const vv::graph<Type, WeightT>& g, const Type& s, const bool verbose = true)
{
    // Print adjacency list representation of graph.
    if (verbose)
    {
        std::cout << g << '\n';
    }

    // Set start time.
    const auto start = std::chrono::steady_clock::now();
    // Call Levit's algorithm.
    const auto result = vv::levit_algorithm(g, s);
    // Stop timer and calculate result's time.
    const std::chrono::duration<double> duration = std::chrono::steady_clock::now() - start;

    // Print results.
    std::cout << "Calculation time: " << duration.count() << " ms\n";
    if (verbose)
    {
        std::cout << "Final vector of weights:\n";
        utils::println_pair(std::cout, result);
    }
    std::cout << "\n\n";

    return duration;
}

} // namespace detail

std::chrono::duration<double> test_1(const int s = 3, const bool verbose = true)
{
    // Number of nodes in the graph. NOT hardcoded value, need to help hash map to reserve proper
    // number of buckets.
    constexpr std::size_t N = 6;

    assert(0 <= s && s < N);

    // Vector of graph edges.
    std::vector<vv::edge<int, long long>> edges =
    {
        // (u, v, w) -> edge from u to v having weight w.
        { 0, 1, 6 }, { 1, 2, 7 }, { 2, 0, 5 }, { 2, 1, 4 },
        { 3, 2, 10 }, { 4, 5, 1 }, { 5, 4, 3 }
    };

    // Define bilateral graph property.
    constexpr bool bilateral = false;

    // Construct graph.
    vv::graph<int, long long> g(edges, bilateral, N);

    return detail::_make_test(g, s, verbose);
}

std::chrono::duration<double> test_2(const int vertices_number = 30, const int s = 0,
                                     const bool verbose = true)
{
    assert(0 <= s && s < vertices_number * 3 + 1);
    const auto g = gen::generate_tricky_case(vertices_number);
    return detail::_make_test(g, s, verbose);
}

std::chrono::duration<double> test_3(const int vertices_number = 10, const bool verbose = true)
{
    const auto g = gen::generate_rand_graph(vertices_number);
    return detail::_make_test(g, utils::take_accidentally(g.data()).first, verbose);
}

void time_tests_series()
{
    std::cout << "Execute default test suit\n";
    tests::test_1();

    constexpr int s = 0; // Start vertex.
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
        std::cout << "Size: " << i << '\n';
        const auto result = tests::test_2(i, s, verbose);
        time_results.emplace_back(i, result.count());
    }
    utils::out_data("levit_complex_case.txt", "1p", "def",
                    "Tricky case for Levit's algorithm", "Number of vertex", "Completion time, ms",
                    time_results);

    // Random test cases.
    std::cout << "Execute random test suit\n";
    time_results.clear();
    for (const auto& i : vertices_number)
    {
        std::cout << "Size: " << i << '\n';
        const auto result = tests::test_3(i, verbose);
        time_results.emplace_back(i, result.count());
    }
    utils::out_data("levit_rand_tests.txt", "1p", "def",
                    "Random tests for Levit's algorithm", "Number of vertex", "Completion time, ms",
                    time_results);
}

void average_time_tests()
{
    constexpr int s = 0; // Start vertex.
    constexpr bool verbose = false; // Output result flag.
    // Create tests array.
    constexpr std::array vertices_number{ 10, 20, 40, 80, 160, 320, 640, 1280, 2560 };
    constexpr int launches_number = 10;

    // Tricky case for Levit's algorithm which can cause exponential complexity if implementation
    // merges queues M1' and M1'' into one M1.
    std::cout << "Execute tricky test suit\n";
    std::vector<std::pair<double, double>> time_results;
    time_results.reserve(vertices_number.size());
    for (const auto& i : vertices_number)
    {
        const int size = i * 3 + 1;
        std::cout << "Size: " << size << '\n';
        double result = 0;
        std::vector<std::pair<double, double>> one_test_results;
        one_test_results.reserve(launches_number);
        for (int j = 1; j <= launches_number; ++j)
        {
            const auto launch_result = tests::test_2(size, s, verbose).count();
            result += launch_result;
            one_test_results.emplace_back(j, launch_result);
        }

        const auto number = std::to_string(size);
        utils::out_data("levit_complex_case_average_" + number + ".txt", "1p", "def",
                        "Tricky case for Levit's algorithm, single test with n=" + number,
                        "Number of vertex", "Completion time",
                        one_test_results);

        time_results.emplace_back(size, result / launches_number);
    }
    utils::out_data("levit_complex_case_average.txt", "1p", "def",
                    "Tricky case for Levit's algorithm", "Number of vertex", "Completion time, ms",
                    time_results);

    // Random test cases.
    std::cout << "Execute random test suit\n";
    time_results.clear();
    for (const auto& i : vertices_number)
    {
        std::cout << "Size: " << i << '\n';
        double result = 0;
        std::vector<std::pair<double, double>> one_test_results;
        one_test_results.reserve(launches_number);
        for (int j = 1; j <= launches_number; ++j)
        {
            const auto launch_result = tests::test_3(i, verbose).count();
            result += launch_result;
            one_test_results.emplace_back(j, launch_result);
        }

        const auto number = std::to_string(i);
        utils::out_data("levit_rand_tests_average_" + number + ".txt", "1p", "def",
                        "Random tests for Levit's algorithm, single test with n=" + number,
                        "Number of vertex", "Completion time",
                        one_test_results);

        time_results.emplace_back(i, result / launches_number);
    }
    utils::out_data("levit_rand_tests_average.txt", "1p", "def",
                    "Random tests for Levit's algorithm", "Number of vertex", "Completion time, ms",
                    time_results);
}

void average_time_tests_series()
{
    constexpr int s = 0; // Start vertex.
    constexpr bool verbose = false; // Output result flag.
    // Create tests array.
    constexpr int start_value = 10;
    constexpr int end_value = 2560;
    constexpr int launches_number = 10;

    // Tricky case for Levit's algorithm which can cause exponential complexity if implementation
    // merges queues M1' and M1'' into one M1.
    std::cout << "Execute tricky test suit\n";
    std::vector<std::pair<double, double>> time_results;
    time_results.reserve(end_value / start_value);
    //for (int i = start_value; i <= end_value; i += 10)
    //{
    //    const int size = i * 3 + 1;
    //    std::cout << "Size: " << size << '\n';
    //    double result = 0;
    //    for (int j = 1; j <= launches_number; ++j)
    //    {
    //        const auto launch_result = tests::test_2(size, s, verbose).count();
    //        result += launch_result;
    //    }

    //    time_results.emplace_back(size, result / launches_number);
    //}
    //utils::out_data("levit_complex_case_average_series.txt", "1p", "def",
    //                "Tricky case for Levit's algorithm", "Number of vertex", "Completion time, ms",
    //                time_results);

    // Random test cases.
    std::cout << "Execute random test suit\n";
    time_results.clear();
    for (int i = start_value; i <= end_value; i += 10)
    {
        std::cout << "Size: " << i << '\n';
        double result = 0;
        for (int j = 1; j <= launches_number; ++j)
        {
            const auto launch_result = tests::test_3(i, verbose).count();
            result += launch_result;
        }

        time_results.emplace_back(i, result / launches_number);
    }
    utils::out_data("levit_rand_tests_average_series.txt", "1p", "def",
                    "Random tests for Levit's algorithm", "Number of vertex", "Completion time, ms",
                    time_results);
}

void create_theoretical_data()
{
    std::cout << "Create theoretical results\n";
    // Create tests array.
    constexpr std::array vertices_number{ 10, 20, 40, 80, 160, 320, 640, 1280, 2560 };
    constexpr std::array vertices_number2{ 31, 61, 121, 241, 481, 961, 1921, 3841, 7681 };

    const auto average_case = [](const int n) -> std::pair<double, double>
    {
        //return { n, 0.01940832 - 0.0005256873 * n + 0.000006840334 * n * n + 5.826337e-9 * n * n * n };
        //return { n, 0.0004967834 + 0.00003381961 * n + 4.244506e-8 * n * n };
        return { n, 1.22541e-7 * std::pow(n, 2.33116) - 0.796133 };
        //return { n, 8.735825e-7 * std::pow(n, 1.649123) };
    };

    std::vector<std::pair<double, double>> results;
    results.reserve(vertices_number.size());
    for (const auto& i : vertices_number2)
    {
        results.emplace_back(average_case(i));
    }
    utils::out_data("levit_theory_axb.txt", "1p", "def",
                    "Theoretical results", "Number of vertex", "Completion time, ms",
                    results);
}

} // namespace tests
