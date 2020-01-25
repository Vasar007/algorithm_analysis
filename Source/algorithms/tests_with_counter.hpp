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

////////////////////////////////////////////////////////////////////////////////////////////////////

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

////////////////////////////////////////////////////////////////////////////////////////////////////

int test_1(const int vertices_number = 30, const int start_vertex = 0, const bool verbose = true)
{
    assert(0 <= start_vertex && start_vertex < vertices_number * 3 + 1);
    const auto graph_instance = gen::generate_tricky_case(vertices_number);
    return details::_make_test(graph_instance, start_vertex, verbose);
}

////////////////////////////////////////////////////////////////////////////////////////////////////

int test_2(const int vertices_number = 10, const bool verbose = true)
{
    const auto graph_instance = gen::generate_full_graph(vertices_number);
    //const auto graph_instance = vv::read_csv_and_add_weights("test.csv");
    return details::_make_test(
        graph_instance, utils::take_accidentally(graph_instance.data()).first, verbose
    );
}

////////////////////////////////////////////////////////////////////////////////////////////////////

int test_array_sort(const int elements_number = 10, [[maybe_unused]] const bool verbose = true)
{
    auto arr = gen_array::create_random_array(elements_number);
    return details::_make_test(arr);
}

////////////////////////////////////////////////////////////////////////////////////////////////////

/// Test section.

void average_operation_number_tests_series(const utils::parameters_pack params)
{
    if (!params.is_valid)
    {
        // Return control if parameters are invalid.
        return;
    }

    constexpr bool verbose = false; // Output result flag.

    // Deconstruct parameters pack.
    const utils::algorithm_type type = params.type;
    const auto func = [type](const int arg, const bool verbose) -> int
    {
        switch (type)
        {
            case utils::algorithm_type::pallotino:
                return test_2(arg, verbose);

            case utils::algorithm_type::insertion_sort:
                return test_array_sort(arg, verbose);

            default:
                return 0;
        }
    };

    const int start_value = params.start_value;
    const int end_value = params.end_value;
    const int launches_number = params.launches_number;
    const int step = params.step;

    // Create container and execute tests.
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
            const auto launch_result = func(i, verbose);
            result += launch_result;
            one_test_results.emplace_back(launch_result);
            std::cout << "Execution: " << j << "; Operations number: " << launch_result << '\n';
        }

        const auto number = std::to_string(i);
        const std::string output_filename(params.output_filename_pattern.data() + number + ".txt");
        utils::out_data(output_filename, "1p", "def",
                        "Random tests for algorithm analysis, single test with n=" + number,
                        "Number of vertex", "Operations number",
                        one_test_results);
        operation_results.emplace_back(i, result / launches_number);
    }
    const std::string output_filename_series(params.output_filename_pattern.data() + std::string("series.txt"));
    utils::out_data(output_filename_series, "1p", "def",
                    "Random tests for algorithm analysis", "Number of vertex", "Operations number",
                    operation_results);
}

} // namespace tests_with_counter
