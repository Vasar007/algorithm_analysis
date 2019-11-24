// Copyright (C) 2018 Vasily Vasilyev (vasar007@yandex.ru)

#include "tests_with_time.hpp"
#include "tests_with_counter.hpp"
#include "utils.hpp"


int main()
{
    tests_with_time::time_tests_series();
    tests_with_time::average_time_tests_series();
    tests_with_time::average_time_tests_relative();
    tests_with_time::create_theoretical_data();

    tests_with_counter::average_operation_number_tests_series();

    utils::pause();

    return 0;
}
