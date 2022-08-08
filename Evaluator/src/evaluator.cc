#include <cctype>
#include <cstring>
#include <iostream>
#include <map>
#include <string>
#include <vector>
#include <algorithm>
#include "ctype.h"
#include <optional>
#include <functional>
#include <sstream>

enum ActionsEnum { EVALUATE, VERIFY };
std::map<std::string, ActionsEnum> mapAliasToAction{
            {"evaluate", EVALUATE},
            {"eval", EVALUATE},
            {"e", EVALUATE},
            {"verify", VERIFY},
            {"ver", VERIFY},
            {"v", VERIFY},
        };

std::vector<std::string> split(const std::string& target, char c)
{
	std::string temp;
	std::stringstream stringstream { target };
	std::vector<std::string> result;

	while (std::getline(stringstream, temp, c)) {
		result.push_back(temp);
	}

	return result;
}

std::optional<std::string> evaluate(std::string language, std::string code, std::vector<std::string> input, std::string path_to_evaluators) {
    std::string exec_path;
    if (path_to_evaluators[path_to_evaluators.size() - 1] == '/') {
        exec_path = path_to_evaluators + language;
    } else {
        exec_path = path_to_evaluators + '/' + language;
    }
    exec_path += "/evaluator";

}

bool verify(std::string language, std::string code, std::string path_to_evaluators) {
    std::string exec_path;
    if (path_to_evaluators[path_to_evaluators.size() - 1] == '/') {
        exec_path = path_to_evaluators + language;
    } else {
        exec_path = path_to_evaluators + '/' + language;
    }
    exec_path += "/evaluator";

}


int main(int argc, char** argv)
{
    if (argc <= 2) {
        std::cout << "Wrong number of arguments provided. No action found." << std::endl;
        return -1;
    }

    auto operation = std::string(argv[1]);
    transform(operation.begin(), operation.end(), operation.begin(), ::tolower);

    std::string language, code, path_to_evaluators;
    std::vector<std::string> input;
    std::optional<std::string> result_evaluate;
    bool result_verify;
    std::optional<std::string> result_;

    switch(mapAliasToAction[operation]) {
        case EVALUATE:

        if (argc != 6) {
            std::cout << 
            "Wrong number of arguments provided for the EVALUATE action." << std::endl
            << "Should provide: EVALUATE LANGUAGE CODE INPUT PATH_TO_EVALUATORS";
            return -1;
        }

        language = std::string(argv[2]);
        code = std::string(argv[3]);
        input = split(std::string(argv[4]), '\n');
        path_to_evaluators = std::string(argv[5]);

        result_evaluate = evaluate(language, code, input, path_to_evaluators);
        if(result_evaluate) {
            std::cout << result_evaluate.value() << std::endl;
            return 0;
        }
        return -1;

        case VERIFY:
        
        if (argc != 5) {
            std::cout << 
            "Wrong number of arguments provided for the EVALUATE action." << std::endl
            << "Should provide: VERIFY LANGUAGE CODE PATH_TO_EVALUATORS";
            return -1;
        }

        language = std::string(argv[2]);
        code = std::string(argv[3]);
        path_to_evaluators = std::string(argv[5]);

        result_verify = verify(language, code, path_to_evaluators);
        if(result_verify) {
            std::cout << result_verify << std::endl;
            return 0;
        }
        return -1;
        break;

        default:
            std::cout << "Action is invalid. Should be EVALUATE | VERIFY." << std::endl;
            return -1;
        break;
    }
    return 0;
}