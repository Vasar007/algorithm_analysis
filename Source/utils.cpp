// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#include <cassert>
#include <ctime>
#include <iostream>
#include <iterator>
#include <fstream>
#include <random>
#include <string>
#include <string_view>
#include <vector>
#include <utility>

#include "utils.hpp"


namespace utils
{

std::mt19937 create_random_engine()
{
    // Obtain a time-based seed:
    const auto seed = static_cast<unsigned long>(std::time(nullptr));
    return std::mt19937(seed);
}

void pause(const std::string_view message)
{
    do
    {
        std::cout << message;
    }
    while (std::cin.get() != '\n');
}

void pause_clear(const std::string_view message)
{
    std::cout << message << std::flush;
    std::cin.seekg(0u, std::ios::end);
    std::cin.clear();
    std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
}

void out_data(const std::string& file_name, const std::string_view mode,
              const std::string_view param, const std::string_view title,
              const std::string_view x_label, const std::string_view y_label,
              const std::vector<double>& data)
{
    if (data.empty())
    {
        std::cout << "ERROR: empty data to process for file " << file_name << ".\n";
        return;
    }

    std::ofstream out_file(file_name);
    out_file << mode << '|' << param << '|' << title << '|' << x_label << '|' << y_label << '\n';
    for (const auto& x : data)
    {
        out_file << x << '\n';
    }
}

void out_data(const std::string& file_name, const std::string_view mode,
              const std::string_view param, const std::string_view title,
              const std::string_view x_label, const std::string_view y_label,
              const std::vector<std::pair<double, double>>& data)
{
    if (data.empty())
    {
        std::cout << "ERROR: empty data to process for file " << file_name << ".\n";
        return;
    }

    std::ofstream out_file(file_name);
    out_file << mode << '|' << param << '|' << title << '|' << x_label << '|' << y_label << '\n';
    for (const auto& [x, y] : data)
    {
        out_file << x << ' ' << y << '\n';
    }
}

void out_data(const std::string& file_name, const std::string_view mode,
              const std::string_view param, const std::string_view title,
              const std::string_view x_label, const std::string_view y_label,
              const std::vector<std::pair<double, double>>& data_1,
              const std::vector<std::pair<double, double>>& data_2)
{
    if (data_1.empty() || data_2.empty())
    {
        std::cout << "ERROR: empty data to process for file " << file_name << ".\n";
        return;
    }

    std::ofstream out_file(file_name);
    out_file << mode << '|' << param << '|' << title << '|' << x_label << '|' << y_label << '\n';
    for (const auto& [x, y] : data_1)
    {
        out_file << x << ' ' << y << '\n';
    }
    out_file << "#\n";
    for (const auto [x, y] : data_2)
    {
        out_file << x << ' ' << y << '\n';
    }
}

} // namespace utils
