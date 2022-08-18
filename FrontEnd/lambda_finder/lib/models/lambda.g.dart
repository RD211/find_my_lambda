// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'lambda.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_$_Lambda _$$_LambdaFromJson(Map<String, dynamic> json) => _$_Lambda(
      id: json['id'] as int?,
      name: json['name'] as String,
      description: json['description'] as String,
      email: json['email'] as String,
      programmingLanguage: json['programmingLanguage'] as String,
      code: json['code'] as String,
      inputType: json['inputType'] as String?,
      returnType: json['returnType'] as String?,
      uploadDate: json['uploadDate'] == null
          ? null
          : DateTime.parse(json['uploadDate'] as String),
      timesUsed: json['timesUsed'] as int?,
      confirmed: json['confirmed'] as bool?,
      verified: json['verified'] as bool?,
      likes: json['likes'] as int?,
    );

Map<String, dynamic> _$$_LambdaToJson(_$_Lambda instance) => <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'description': instance.description,
      'email': instance.email,
      'programmingLanguage': instance.programmingLanguage,
      'code': instance.code,
      'inputType': instance.inputType,
      'returnType': instance.returnType,
      'uploadDate': instance.uploadDate?.toIso8601String(),
      'timesUsed': instance.timesUsed,
      'confirmed': instance.confirmed,
      'verified': instance.verified,
      'likes': instance.likes,
    };
