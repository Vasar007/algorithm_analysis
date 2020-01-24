// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#pragma once

#include <algorithm>
#include <cassert>
#include <iostream>
#include <iterator>
#include <limits>
#include <random>
#include <string>
#include <string_view>
#include <type_traits>
#include <vector>
#include <utility>


namespace utils
{

constexpr int UPPER_BORDER = 10'000;

std::mt19937 create_random_engine();
inline std::mt19937 RANDOM_ENGINE = create_random_engine();

template <class Type>
[[nodiscard]] std::enable_if_t<std::is_arithmetic_v<Type>, Type>
random_number(const Type& a = 0, const Type& b = std::numeric_limits<Type>::max())
{
    assert(a <= b); // According to doc, we can get values in segment [a, b].
    std::uniform_int_distribution<Type> distribution(a, b);
    return distribution(RANDOM_ENGINE);
}

template <class Container>
[[nodiscard]] typename Container::value_type take_accidentally(const Container& cont)
{
    std::uniform_int_distribution<std::size_t> distribution(0, cont.size() - 1);
    return *std::next(std::begin(cont), distribution(RANDOM_ENGINE));
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
    out << '\n';
}

template <class OutputStream, class Container>
void print_pair(OutputStream& out, const Container& container)
{
    for (const auto& [first, second] : container)
    {
        out << first << ": " << second << '\n';
    }
}

template <class OutputStream, class Container>
void println_pair(OutputStream& out, const Container& container)
{
    print_pair(out, container);
    std::cout << '\n';
}

void pause(const std::string_view message = "\nPress the Enter key to continue...");

void pause_clear(const std::string_view message = "Press ENTER to continue...");

void out_data(const std::string_view file_name, const std::string_view mode,
              const std::string_view param, const std::string_view title,
              const std::string_view x_label, const std::string_view y_label,
              const std::vector<double>& data);

void out_data(const std::string_view file_name, const std::string_view mode,
              const std::string_view param, const std::string_view title,
              const std::string_view x_label, const std::string_view y_label,
              const std::vector<std::pair<double, double>>& data);

void out_data(const std::string_view file_name, const std::string_view mode,
              const std::string_view param, const std::string_view title,
              const std::string_view x_label, const std::string_view y_label,
              const std::vector<std::pair<double, double>>& data_1,
              const std::vector<std::pair<double, double>>& data_2);

} // namespace utils
