import 'package:dio/dio.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:lambda_finder/environment_config.dart';

import '../models/lambda.dart';

final lambdaServiceProvider = StateProvider((ref) => LambdaService());

class LambdaService {
  final Dio _dio = Dio();

  final _baseUrl = EnvironmentConfig.BASE_URL;

  Future<Lambda> postLambda(Lambda lambda) async {
    Response response = await _dio.post('$_baseUrl/Lambda', data: {
      'name': lambda.name,
      'description': lambda.description,
      'email': lambda.email,
      'programmingLanguage': lambda.programmingLanguage,
      'code': lambda.code
    });
    return Lambda.fromJson(response.data);
  }
}
