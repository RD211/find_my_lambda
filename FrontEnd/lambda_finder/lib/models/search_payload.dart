import 'package:freezed_annotation/freezed_annotation.dart';

part 'search_payload.freezed.dart';
part 'search_payload.g.dart';

@freezed
class SearchPayload with _$SearchPayload {
  const factory SearchPayload({
    required List<String> inputs,
    required List<String> results,
  }) = _SearchPayload;

  factory SearchPayload.fromJson(Map<String, Object?> json) =>
      _$SearchPayloadFromJson(json);
}
