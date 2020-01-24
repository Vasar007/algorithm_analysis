// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#include <cassert>
#include <ctime>
#include <cstdio>
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

void std_output(const std::string_view str)
{
    std::cout << str;
}

void fast_output(const std::string_view str) noexcept
{
    if (const int returnValue = std::puts(str.data());
        returnValue == EOF)
    {
        // POSIX requires that errno is set.
        std::perror("puts() failed.");
    }
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

void out_data(const std::string_view file_name, const std::string_view mode,
              const std::string_view param, const std::string_view title,
              const std::string_view x_label, const std::string_view y_label,
              const std::vector<double>& data)
{
    if (data.empty())
    {
        std::cout << "ERROR: empty data to process for file '" << file_name << "'.\n";
        return;
    }

    std::ofstream out_file(file_name.data());
    out_file << mode << '|' << param << '|' << title << '|' << x_label << '|' << y_label << '\n';
    for (const auto& x : data)
    {
        out_file << x << '\n';
    }

    out_file.flush();
}

void out_data(const std::string_view file_name, const std::string_view mode,
              const std::string_view param, const std::string_view title,
              const std::string_view x_label, const std::string_view y_label,
              const std::vector<std::pair<double, double>>& data)
{
    if (data.empty())
    {
        std::cout << "ERROR: empty data to process for file '" << file_name << "'.\n";
        return;
    }

    std::ofstream out_file(file_name.data());
    out_file << mode << '|' << param << '|' << title << '|' << x_label << '|' << y_label << '\n';
    for (const auto& [x, y] : data)
    {
        out_file << x << ' ' << y << '\n';
    }

    out_file.flush();
}

void out_data(const std::string_view file_name, const std::string_view mode,
              const std::string_view param, const std::string_view title,
              const std::string_view x_label, const std::string_view y_label,
              const std::vector<std::pair<double, double>>& data_1,
              const std::vector<std::pair<double, double>>& data_2)
{
    if (data_1.empty() || data_2.empty())
    {
        std::cout << "ERROR: empty data to process for file '" << file_name << "'.\n";
        return;
    }

    std::ofstream out_file(file_name.data());
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

    out_file.flush();
}

int try_parse_int(const std::string_view str) noexcept
{
    int result;
    if (const auto [ptr, ec] = std::from_chars(str.data(), str.data() + str.size(), result);
        ec == std::errc())
    {
        return result;
    }

    std::cout << "ERROR: invalid argument to parse '" << str << "'.\n";
    return int{};
}

} // namespace utils
