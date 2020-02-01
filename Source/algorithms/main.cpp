// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#include <array>
#include <string_view>
#include <cstdlib>

#include "common_logic.hpp"
#include "tests_with_time.hpp"
#include "tests_with_counter.hpp"
#include "utils.hpp"


int main(const int argc, const char* const * const argv)
{
    try
    {
        // The first argument is path to executed file.

        if (constexpr int no_additional_arguments = 1;
            argc == no_additional_arguments)
        {
            const auto params = utils::parameters_pack::create_default();
            tests_with_counter::average_operation_number_tests_series(params);
        }
        else if (constexpr int time_series_launch = 2;
                 argc == time_series_launch && argv[1] == "time")
        {
            tests_with_time::time_tests_series();
            tests_with_time::average_time_tests_series();
            tests_with_time::average_time_tests_relative();
            common_logic::create_theoretical_data();
        }
        else if (constexpr int parametric_launch = utils::parameters_pack::expected_args_number + 1;
                 argc == parametric_launch)
        {
            const std::array<std::string_view, utils::parameters_pack::expected_args_number> args
            {
                argv[1], argv[2], argv[3], argv[4], argv[5], argv[6]
            };
            const auto params = utils::parameters_pack::try_parse(args);
            tests_with_counter::average_operation_number_tests_series(params);
        }
        else
        {
            constexpr std::string_view error_message = "Invalid arguments number. Usages:\n"
                "- Default launch (without any arguments): <program_name>\n"
                "- Time series launch: <program_name> time"
                "- Empirical analysis launch: <program_name> <algorithm_type> <start_value> <end_value> <launches_number> <step> <output_filename_pattern>\n";
            utils::std_output(error_message);

            return EXIT_FAILURE;
        }
    }
    catch (const std::exception & ex)
    {
        constexpr std::string_view exception_message = "Exception occured: ";
        utils::std_output(exception_message);
        utils::std_output(ex.what());
        utils::std_output("\n");

        return EXIT_FAILURE;
    }
    catch (...)
    {
        constexpr std::string_view exception_message = "Unknown exception occured.\n";
        utils::std_output(exception_message);

        return EXIT_FAILURE;
    }

    return EXIT_SUCCESS;
}
