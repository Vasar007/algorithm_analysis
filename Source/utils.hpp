// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#pragma once

#include <algorithm>
#include <array>
#include <cassert>
#include <charconv>
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
void print_container(OutputStream& out, const Container& container)
{
    std::copy(std::begin(container), std::end(container),
              std::ostream_iterator<typename Container::value_type>(out, " "));
}

template <class OutputStream, class Container>
void println_container(OutputStream& out, const Container& container)
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
    out << '\n';
}

/**
 * \brief         Print string to standart outstream.
 * \param[in] str String to print.
 */
void std_output(const std::string_view str);


/**
 * \brief         Fast print string to outstream.
 * \param[in] str String to print.
 */
void fast_output(const std::string_view str) noexcept;

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

int try_parse_int(const std::string_view args) noexcept;

/**
 * \brief            Splits string by delimeter characters and don't include them.
 * \tparam Container Type of the contatiner for output.
 * \param[in] str    String to split.
 * \param[in] delims Delimiter characters (not STRINGS!).
 */
template <class Container>
Container split(const std::string_view str, const std::string_view delims,
                const std::size_t to_reserve = 0) noexcept
{
    std::size_t previous = 0;
    std::size_t current = str.find_first_of(delims);
    if (current == std::string::npos)
    {
        return Container{};
    }

    Container cont{};
    if (to_reserve > 0)
    {
        cont.reserve(to_reserve);
    }

    while (current != std::string::npos)
    {
        cont.emplace_back(str.substr(previous, current - previous));
        previous = current + 1;
        current = str.find_first_of(delims, previous);
    }

    cont.emplace_back(str.substr(previous, current - previous));

    return cont;
}

/**
 * \brief            Splits string by delimeter string and don't includes it.
 * \tparam Container Type of the contatiner for output.
 * \param[in] str    String to split.
 * \param[in] delim	 Delimiter string (not CHARACTERS!).
 */
template <class Container>
Container fsplit(const std::string_view str, const std::string_view delim,
                 const std::size_t to_reserve = 0) noexcept
{
    const std::size_t length = delim.size();
    std::size_t previous = 0;
    std::size_t current = str.find(delim);
    if (current == std::string::npos)
    {
        return Container{};
    }

    Container cont{};
    if (to_reserve > 0)
    {
        cont.reserve(to_reserve);
    }

    while (current != std::string::npos)
    {
        cont.emplace_back(str.substr(previous, current - previous));
        previous = current + length;
        current = str.find(delim, previous);
    }

    cont.emplace_back(str.substr(previous, current - previous));

    return cont;
}

struct parameters_pack
{
    int start_value;
    int end_value;
    int launches_number;
    int step;

    constexpr parameters_pack() noexcept
    : start_value(0)
    , end_value(0)
    , launches_number(0)
    , step(0)
    {
    }

    constexpr parameters_pack(int start_value, int end_value, int launches_number, int step) noexcept
    : start_value(start_value)
    , end_value(end_value)
    , launches_number(launches_number)
    , step(step)
    {
    }

    static constexpr parameters_pack create_default() noexcept
    {
        return parameters_pack();
    }

    static parameters_pack try_parse(const std::array<std::string_view, 4>& args) noexcept
    {
        const int start_value = try_parse_int(args[0]);
        const int end_value = try_parse_int(args[1]);
        const int launches_number = try_parse_int(args[2]);
        const int step = try_parse_int(args[3]);

        return parameters_pack(start_value, end_value, launches_number, step);
    }
};

} // namespace utils
