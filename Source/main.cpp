// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#include "common_logic.hpp"
#include "tests_with_time.hpp"
#include "tests_with_counter.hpp"
#include "utils.hpp"


int main(const int argc, const char* const * const argv)
{
    if (constexpr int no_arguments = 1;
        argc == no_arguments)
    {
        //tests_with_time::time_tests_series();
        //tests_with_time::average_time_tests_series();
        //tests_with_time::average_time_tests_relative();
        //tests_with_time::create_theoretical_data();

        tests_with_counter::average_operation_number_tests_series();
    }
    else if (constexpr int parametric_launch = 3;
             argc == parametric_launch)
    {
    }
    else
    {
        std::cout << "Invalid arguments number. Usage: " << argv[0]
                  << " <param_1> <param_2>" << std::endl;
    }

    utils::pause();

    return 0;
}
