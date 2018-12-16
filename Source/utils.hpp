// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#pragma once

#include <cassert>
#include <ctime>
#include <iostream>
#include <iterator>
#include <fstream>
#include <random>
#include <string>
#include <string_view>
#include <type_traits>
#include <vector>
#include <utility>


namespace utils
{

namespace
{

    std::default_random_engine create_random_engine()
    {
        // Obtain a time-based seed:
        const auto seed = static_cast<unsigned long>(std::time(nullptr));
        return std::default_random_engine(seed);
    }

    auto RANDOM_ENGINE = create_random_engine();

} // anonymous namespace

template <class Type>
[[nodiscard]] std::enable_if_t<std::is_arithmetic_v<Type>, Type>
random_number(const Type& a = 0, const Type& b = std::numeric_limits<Type>::max())
{
    assert(a <= b);
    std::uniform_int_distribution<Type> distr(a, b);
    return distr(RANDOM_ENGINE);
}

template <class Container>
[[nodiscard]] typename Container::value_type take_accidentally(const Container& cont)
{
    std::uniform_int_distribution<std::size_t> dis(0, cont.size() - 1);
    return *std::next(std::begin(cont), dis(RANDOM_ENGINE));
}

template <class OutputStream, class Container>
void print(OutputStream& out, const Container& container)
{
    std::copy(std::begin(container), std::end(container),
              std::ostream_iterator<typename Container::value_type>(out, " "));
}

template <class OutputStream, class Container>
void println(OutputStream& out, const Container& container)
{
    print(out, container);
    std::cout << '\n';
}

template <class OutputStream, class Container>
void print_pair(OutputStream& out, const Container& container)
{
    for (const auto&[first, second] : container)
    {
        std::cout << first << ": " << second << '\n';
    }
}

template <class OutputStream, class Container>
void println_pair(OutputStream& out, const Container& container)
{
    print_pair(out, container);
    std::cout << '\n';
}

void pause(const std::string_view message = "\nPress the Enter key to continue...")
{
    do
    {
        std::cout << message;
    }
    while (std::cin.get() != '\n');
}


void pause_clear(const std::string_view message = "Press ENTER to continue...")
{
    std::cout << message << std::flush;
    std::cin.seekg(0u, std::ios::end);
    std::cin.clear();
    std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
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
