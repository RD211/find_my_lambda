import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:json_annotation/json_annotation.dart';
import 'package:lambda_finder/environment_config.dart';
import 'package:lambda_finder/models/search_payload.dart';

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

  Future<List<Lambda>> searchLambda(SearchPayload searchPayload) async {
    Response response = await _dio.post(
      '$_baseUrl/Lambda/search',
      data: searchPayload.toJson(),
    );

    var eles = (response.data as List<dynamic>)
        .map((e) => Lambda.fromJson(e))
        .toList();
    return eles;
  }

  Future<Lambda> getLambda(int lambdaId) async {
    Response response = await _dio.get('$_baseUrl/Lambda?id=$lambdaId');
    return Lambda.fromJson(response.data);
  }
}
