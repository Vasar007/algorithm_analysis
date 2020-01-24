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

////////////////////////////////////////////////////////////////////////////////////////////////////

std::mt19937 create_random_engine();
inline std::mt19937 RANDOM_ENGINE = create_random_engine();

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

////////////////////////////////////////////////////////////////////////////////////////////////////

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

////////////////////////////////////////////////////////////////////////////////////////////////////

enum class algorithm_type : int
{
    unknown = -1,
    pallotino = 0,
    insertion_sort = 1,
    algorithms_count = 2
};

constexpr bool enum_is_defined(const int type_as_int)
{
    const auto min_type_value = static_cast<int>(algorithm_type::unknown);
    const auto max_type_value = static_cast<int>(algorithm_type::algorithms_count);

    if (min_type_value < type_as_int && type_as_int < max_type_value)
    {
        return true;
    }

    return false;
}

constexpr bool enum_is_defined(const algorithm_type type)
{
    const auto type_as_int = static_cast<int>(type);
    return enum_is_defined(type_as_int);
}

////////////////////////////////////////////////////////////////////////////////////////////////////

struct parameters_pack
{
    static constexpr int expected_args_number = 5;

    algorithm_type type;
    int start_value;
    int end_value;
    int launches_number;
    int step;
    bool is_valid;


    constexpr parameters_pack() noexcept
    : type(algorithm_type::unknown)
    , start_value(0)
    , end_value(0)
    , launches_number(0)
    , step(0)
    , is_valid(false)
    {
    }

    parameters_pack(const int type_as_int, const int start_value, const int end_value,
                    const int launches_number, const int step)
    : start_value(start_value)
    , end_value(end_value)
    , launches_number(launches_number)
    , step(step)
    , is_valid(true)
    {
        if (enum_is_defined(type_as_int))
        {
            this->type = static_cast<algorithm_type>(type_as_int);
        }
        else
        {
            constexpr auto min_type_value = static_cast<int>(algorithm_type::unknown);
            constexpr auto max_type_value = static_cast<int>(algorithm_type::algorithms_count);

            std::cout << "ERROR: 'algorithm_type' is out of range. Expected range: ["
                      << (min_type_value + 1) << ", " << (max_type_value - 1)
                      << "], actual value: " << type_as_int << ".\n";

            this->type = algorithm_type::unknown;
            this->is_valid = false;
        }

        // Integer overflow should be contriolled by caller.
        constexpr auto max_int = std::numeric_limits<decltype(start_value)>::max();

        if (start_value <= 0)
        {
            std::cout << "ERROR: 'start_value' is out of range. Expected range: [1, "
                      << max_int << "], actual value: " << start_value << ".\n";
            this->is_valid = false;
        }

        if (end_value <= 0 || end_value < start_value)
        {
            std::cout << "ERROR: 'end_value' is out of range. Expected range: ['start_value', "
                      << max_int << "], actual value: " << end_value << ".\n";
            this->is_valid = false;
        }

        if (launches_number <= 0)
        {
            std::cout << "ERROR: 'launches_number' is out of range. Expected range: [1, "
                      << max_int << "], actual value: " << launches_number << ".\n";
            this->is_valid = false;
        }

        if (step <= 0)
        {
            std::cout << "ERROR: 'step' is out of range. Expected range: [1, "
                      << max_int << "], actual value: " << step << ".\n";
            this->is_valid = false;
        }
    }

    parameters_pack(const algorithm_type type, const int start_value, const int end_value,
                    const int launches_number, const int step)
    : parameters_pack(static_cast<int>(type), start_value, end_value, launches_number, step)
    {
    }

    static parameters_pack create_default()
    {
        return parameters_pack(algorithm_type::pallotino, 80, 80, 200, 10);
    }

    static parameters_pack try_parse(const std::array<std::string_view, 5>& args) noexcept
    {
        const int type_as_int = try_parse_int(args[0]);
        const int start_value = try_parse_int(args[1]);
        const int end_value = try_parse_int(args[2]);
        const int launches_number = try_parse_int(args[3]);
        const int step = try_parse_int(args[4]);
        return parameters_pack(type_as_int, start_value, end_value, launches_number, step);
    }
};

} // namespace utils
