cmake_minimum_required(VERSION 3.8)

# Enable debug symbols by default, must be done before project() statement.
if (NOT CMAKE_BUILD_TYPE)
    set(CMAKE_BUILD_TYPE Debug CACHE STRING "Choose the type of build (Debug or Release)" FORCE)
endif()
# (you can also set it on the command line: -D CMAKE_BUILD_TYPE=Release)

project(algorithm_analysis)

# Set compiler options.
if (MSVC)
    add_compile_options(/W4)
else ()
    add_compile_options(-Wall -Wextra -pedantic)
endif()

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

# Set additional properties for x64 platform.
if (CMAKE_SIZEOF_VOID_P EQUAL 8)
    set_property(GLOBAL PROPERTY FIND_LIBRARY_USE_LIB64_PATHS TRUE)
    set_property(GLOBAL PROPERTY FIND_LIBRARY_USE_LIB32_PATHS FALSE)
else ()
    message(FATAL_ERROR "Unsupported architecture.")
endif()

# Include sub-projects.
add_subdirectory("Source/algorithms")
