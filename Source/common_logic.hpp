// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#pragma once

#include <array>
#include <utility>
#include <vector>

#include "utils.hpp"


namespace common_logic
{

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

} // namespace common_logic
