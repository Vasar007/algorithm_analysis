include_directories(
    ${PROJECT_BINARY_DIR}
)

set(EXECUTABLE_NAME "algorithm_analysis")

add_executable(${EXECUTABLE_NAME}
    utils.hpp
    utils.cpp

    graph.hpp
    generators.hpp
    tests.hpp
    tests_with_counter.hpp

    main.cpp
)
set_target_properties(${EXECUTABLE_NAME} PROPERTIES LINKER_LANGUAGE CXX)
