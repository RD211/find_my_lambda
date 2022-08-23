// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'search_payload.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_$_SearchPayload _$$_SearchPayloadFromJson(Map<String, dynamic> json) =>
    _$_SearchPayload(
      inputs:
          (json['inputs'] as List<dynamic>).map((e) => e as String).toList(),
      results:
          (json['results'] as List<dynamic>).map((e) => e as String).toList(),
    );

Map<String, dynamic> _$$_SearchPayloadToJson(_$_SearchPayload instance) =>
    <String, dynamic>{
      'inputs': instance.inputs,
      'results': instance.results,
    };
