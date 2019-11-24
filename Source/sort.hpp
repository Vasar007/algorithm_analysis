#pragma once

#include <algorithm>
#include <cassert>
#include <iterator>
#include <utility>
#include <vector>

#include "utils.hpp"


namespace sort
{

int insertion_sort(std::vector<int>& arr)
{
    const int n = static_cast<int>(arr.size());

    int counter = 0;
    for (int i = 1; i < n; ++i)
    {
        for (int j = i; j > 0 && arr.at(j - 1) > arr.at(j); --j)
        {
            std::swap(arr.at(j - 1), arr.at(j));
            ++counter;
        }
    }

    return counter;
}

} // namespace sort

namespace gen_array
{

std::vector<int> create_random_array(const int n)
{
    assert(n > 0);

    std::vector<int> result(n);
    std::generate(std::begin(result), std::end(result),
                  []() { return utils::random_number<int>(0, 10'000); });

    return result;
}

} // namespace gen_array
