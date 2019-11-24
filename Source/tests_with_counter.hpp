// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#pragma once

#include <array>
#include <cassert>
#include <chrono>
#include <cmath>
#include <iostream>
#include <iterator>
#include <utility>
#include <vector>

#include "generators.hpp"
#include "graph.hpp"
#include "sort.hpp"
#include "utils.hpp"


namespace tests_with_counter
{

namespace details
{

using dmilliseconds = std::chrono::duration<double, std::milli>;

template <class Type, class WeightT = int>
int _make_test(const vv::graph<Type, WeightT>& graph_instance, const Type& start_vertex,
    const bool verbose = true)
{
    // Print adjacency list representation of graph.
    if (verbose)
    {
        std::cout << graph_instance << '\n';
    }

    // Call Levit's algorithm.
    const auto result = vv::levit_algorithm_with_counter(graph_instance, start_vertex);

    return result.second;
}

int _make_test(std::vector<int>& arr)
{
    // Call insertion sort.
    const auto result = sort::insertion_sort(arr);

    return result;
}

} // namespace details

int test_0(const int start_vertex = 3, const bool verbose = true)
{
    // Number of nodes in the graph. NOT hardcoded value, need to help hash map to reserve proper
    // number of buckets.
    constexpr std::size_t N = 6;

    assert(0 <= start_vertex && start_vertex < N);

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
    const vv::graph<int, long long> graph_instance(edges, bilateral, N);

    return details::_make_test(graph_instance, start_vertex, verbose);
}

int test_1(const int vertices_number = 30, const int start_vertex = 0, const bool verbose = true)
{
    assert(0 <= start_vertex && start_vertex < vertices_number * 3 + 1);
    const auto graph_instance = gen::generate_tricky_case(vertices_number);
    return details::_make_test(graph_instance, start_vertex, verbose);
}

int test_2(const int vertices_number = 10, const bool verbose = true)
{
    //const auto graph_instance = gen::generate_tricky_case(vertices_number);
    const auto graph_instance = vv::read_csv_and_add_weights("test.csv");
    return details::_make_test(graph_instance, //static_cast<int>(vertices_number / 2), verbose);
                              utils::take_accidentally(graph_instance.data()).first, verbose);
}

int test_array_sort(const int elements_number = 10, const bool verbose = true)
{
    auto arr = gen_array::create_random_array(elements_number);
    return details::_make_test(arr);
}

/// Test section.

void average_operation_number_tests_series()
{
    constexpr bool verbose = false; // Output result flag.

    constexpr int start_value = 80;
    constexpr int end_value = 320;
    constexpr int launches_number = 200;
    constexpr int step = 10;

    // Tricky case for Levit's algorithm which can cause exponential complexity if implementation
    // merges queues M1' and M1'' into one M1.
    std::vector<std::pair<double, double>> operation_results;
    operation_results.reserve(end_value / start_value);

    // Random test cases.
    std::cout << "Execute random test suit\n";
    for (int i = start_value; i <= end_value; i += step)
    {
        std::cout << "Size: " << i << '\n';
        double result = 0;
        std::vector<double> one_test_results;
        one_test_results.reserve(launches_number);
        for (int j = 1; j <= launches_number; ++j)
        {
            const auto launch_result = test_array_sort(i, verbose);
            result += launch_result;
            one_test_results.emplace_back(launch_result);
            std::cout << "Execution: " << j << "; Operations number: " << launch_result << '\n';
        }

        const auto number = std::to_string(i);
        utils::out_data("rand_tests_average_" + number + ".txt", "1p", "def",
                        "Random tests for algorithm analysis, single test with n=" + number,
                        "Number of vertex", "Operations number",
                        one_test_results);
        operation_results.emplace_back(i, result / launches_number);
    }
    utils::out_data("rand_tests_average_series.txt", "1p", "def",
                    "Random tests for algorithm analysis", "Number of vertex", "Operations number",
                    operation_results);
}


void create_theoretical_data()
{
    std::cout << "Create theoretical results\n";
    // Create tests array.
    constexpr std::array vertices_number{ 10, 20, 40, 80, 160, 320, 640, 1280, 2560 };
    constexpr std::array vertices_number2{ 31, 61, 121, 241, 481, 961, 1921 };

    const auto average_case = [](const int n) -> std::pair<double, double>
    {
        //return { n, 5.337284676655264e-7 * std::pow(n, 1.7064882767951628) + 0.005903886140516 };
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

} // namespace tests_with_counter
