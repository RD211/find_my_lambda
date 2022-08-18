import 'package:freezed_annotation/freezed_annotation.dart';

part 'lambda.freezed.dart';
part 'lambda.g.dart';

@freezed
class Lambda with _$Lambda {
  const factory Lambda({
    int? id,
    required String name,
    required String description,
    required String email,
    required String programmingLanguage,
    required String code,
    String? inputType,
    String? returnType,
    DateTime? uploadDate,
    int? timesUsed,
    bool? confirmed,
    bool? verified,
    int? likes,
  }) = _Lambda;

  factory Lambda.fromJson(Map<String, Object?> json) => _$LambdaFromJson(json);
}
