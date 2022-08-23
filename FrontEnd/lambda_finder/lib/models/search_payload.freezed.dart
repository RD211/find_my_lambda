// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'search_payload.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more information: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

SearchPayload _$SearchPayloadFromJson(Map<String, dynamic> json) {
  return _SearchPayload.fromJson(json);
}

/// @nodoc
mixin _$SearchPayload {
  List<String> get inputs => throw _privateConstructorUsedError;
  List<String> get results => throw _privateConstructorUsedError;

  Map<String, dynamic> toJson() => throw _privateConstructorUsedError;
  @JsonKey(ignore: true)
  $SearchPayloadCopyWith<SearchPayload> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $SearchPayloadCopyWith<$Res> {
  factory $SearchPayloadCopyWith(
          SearchPayload value, $Res Function(SearchPayload) then) =
      _$SearchPayloadCopyWithImpl<$Res>;
  $Res call({List<String> inputs, List<String> results});
}

/// @nodoc
class _$SearchPayloadCopyWithImpl<$Res>
    implements $SearchPayloadCopyWith<$Res> {
  _$SearchPayloadCopyWithImpl(this._value, this._then);

  final SearchPayload _value;
  // ignore: unused_field
  final $Res Function(SearchPayload) _then;

  @override
  $Res call({
    Object? inputs = freezed,
    Object? results = freezed,
  }) {
    return _then(_value.copyWith(
      inputs: inputs == freezed
          ? _value.inputs
          : inputs // ignore: cast_nullable_to_non_nullable
              as List<String>,
      results: results == freezed
          ? _value.results
          : results // ignore: cast_nullable_to_non_nullable
              as List<String>,
    ));
  }
}

/// @nodoc
abstract class _$$_SearchPayloadCopyWith<$Res>
    implements $SearchPayloadCopyWith<$Res> {
  factory _$$_SearchPayloadCopyWith(
          _$_SearchPayload value, $Res Function(_$_SearchPayload) then) =
      __$$_SearchPayloadCopyWithImpl<$Res>;
  @override
  $Res call({List<String> inputs, List<String> results});
}

/// @nodoc
class __$$_SearchPayloadCopyWithImpl<$Res>
    extends _$SearchPayloadCopyWithImpl<$Res>
    implements _$$_SearchPayloadCopyWith<$Res> {
  __$$_SearchPayloadCopyWithImpl(
      _$_SearchPayload _value, $Res Function(_$_SearchPayload) _then)
      : super(_value, (v) => _then(v as _$_SearchPayload));

  @override
  _$_SearchPayload get _value => super._value as _$_SearchPayload;

  @override
  $Res call({
    Object? inputs = freezed,
    Object? results = freezed,
  }) {
    return _then(_$_SearchPayload(
      inputs: inputs == freezed
          ? _value._inputs
          : inputs // ignore: cast_nullable_to_non_nullable
              as List<String>,
      results: results == freezed
          ? _value._results
          : results // ignore: cast_nullable_to_non_nullable
              as List<String>,
    ));
  }
}

/// @nodoc
@JsonSerializable()
class _$_SearchPayload implements _SearchPayload {
  const _$_SearchPayload(
      {required final List<String> inputs, required final List<String> results})
      : _inputs = inputs,
        _results = results;

  factory _$_SearchPayload.fromJson(Map<String, dynamic> json) =>
      _$$_SearchPayloadFromJson(json);

  final List<String> _inputs;
  @override
  List<String> get inputs {
    // ignore: implicit_dynamic_type
    return EqualUnmodifiableListView(_inputs);
  }

  final List<String> _results;
  @override
  List<String> get results {
    // ignore: implicit_dynamic_type
    return EqualUnmodifiableListView(_results);
  }

  @override
  String toString() {
    return 'SearchPayload(inputs: $inputs, results: $results)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _$_SearchPayload &&
            const DeepCollectionEquality().equals(other._inputs, _inputs) &&
            const DeepCollectionEquality().equals(other._results, _results));
  }

  @JsonKey(ignore: true)
  @override
  int get hashCode => Object.hash(
      runtimeType,
      const DeepCollectionEquality().hash(_inputs),
      const DeepCollectionEquality().hash(_results));

  @JsonKey(ignore: true)
  @override
  _$$_SearchPayloadCopyWith<_$_SearchPayload> get copyWith =>
      __$$_SearchPayloadCopyWithImpl<_$_SearchPayload>(this, _$identity);

  @override
  Map<String, dynamic> toJson() {
    return _$$_SearchPayloadToJson(
      this,
    );
  }
}

abstract class _SearchPayload implements SearchPayload {
  const factory _SearchPayload(
      {required final List<String> inputs,
      required final List<String> results}) = _$_SearchPayload;

  factory _SearchPayload.fromJson(Map<String, dynamic> json) =
      _$_SearchPayload.fromJson;

  @override
  List<String> get inputs;
  @override
  List<String> get results;
  @override
  @JsonKey(ignore: true)
  _$$_SearchPayloadCopyWith<_$_SearchPayload> get copyWith =>
      throw _privateConstructorUsedError;
}
