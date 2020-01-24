// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#include <array>
#include <string_view>

#include "common_logic.hpp"
#include "tests_with_time.hpp"
#include "tests_with_counter.hpp"
#include "utils.hpp"


int main(const int argc, const char* const * const argv)
{
    // The first argument is path to executed file.

    if (constexpr int no_additional_arguments = 1;
        argc == no_additional_arguments)
    {
        tests_with_time::time_tests_series();
        tests_with_time::average_time_tests_series();
        tests_with_time::average_time_tests_relative();
        common_logic::create_theoretical_data();

        const auto params = utils::parameters_pack::create_default();
        tests_with_counter::average_operation_number_tests_series(params);
    }
    else if (constexpr int parametric_launch = utils::parameters_pack::expected_args_number + 1;
             argc == parametric_launch)
    {
        const std::array<std::string_view, 5> args{ argv[1], argv[2], argv[3], argv[4], argv[5] };
        const auto params = utils::parameters_pack::try_parse(args);
        tests_with_counter::average_operation_number_tests_series(params);
    }
    else
    {
        constexpr std::string_view error_message = "Invalid arguments number. Usages:\n"
            "- Without any arguments: <program_name>\n"
            "- With specified arguments: <program_name> <algorithm_type> <start_value> <end_value> <launches_number> <step>\n";

        utils::std_output(error_message);
    }

    utils::pause();

    return 0;
}
