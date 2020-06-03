# clang-gcc-wrapper
Clang/GCC Compilers Wrapper for MSYS2 written in C#

# Goal:
A wrapper that take clang/gcc arguments and forwards it to the original clang/gcc while adding aggressive compiler optimizations, eliminating the need to edit Makefiles for individual projects to tweak the compiler's flags.

# How to use:
1. Compile .sln project, or download precompiled binary.
2. Copy 'Release\clang-wrapper.exe' to MSYS2 Root /mingw64/bin/ (works also with 32-bit /mingw32/bin).
3. Rename original "clang.exe" & "clang++.exe" or "gcc.exe" & "g++.exe" to:
  *clang_.exe & clang++_.exe*
  *gcc_.exe & g++_.exe*
4. Make copies of clang-wrapper.exe and rename them to: *"clang.exe" & "clang++.exe" or "gcc.exe" & "g++.exe"*.
Additional Options:
- you can override the wrapper optimization flags by creating text files *compiler_flags.txt* with custom flags.
- you can enable Clang Full LTO (Full LInk Time Optimizations) by creating a file: *enable_clang_lto*, remove to disable it.

# Future Features:
- ability to wrap and forward from MSVC *clang-cl.exe* to MSYS2 *clang.exe*.
- additional wrappers for the linker and assemblers for better LTO.

*Author: Ahmed Chakhoum*
