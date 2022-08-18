import 'package:code_text_field/code_text_field.dart';
import 'package:flutter/material.dart';
// ignore: depend_on_referenced_packages
import 'package:highlight/highlight_core.dart';
import 'package:flutter_hooks/flutter_hooks.dart';

class _CodeControllerHookCreator {
  const _CodeControllerHookCreator();

  CodeController call(
      {String? text,
      required Map<String, TextStyle> theme,
      required Mode language,
      List<Object?>? keys}) {
    return use(_CodeControllerHook(text, theme, language, keys));
  }
}

const useCodeController = _CodeControllerHookCreator();

class _CodeControllerHook extends Hook<CodeController> {
  const _CodeControllerHook(
    this.initialText,
    this.theme,
    this.language, [
    List<Object?>? keys,
  ]) : super(keys: keys);

  final String? initialText;
  final Map<String, TextStyle> theme;
  final Mode language;
  @override
  _CodeControllerHookState createState() {
    return _CodeControllerHookState();
  }
}

class _CodeControllerHookState
    extends HookState<CodeController, _CodeControllerHook> {
  late final _controller = CodeController(
    text: hook.initialText,
    language: hook.language,
    theme: hook.theme,
  );

  @override
  CodeController build(BuildContext context) => _controller;

  @override
  void dispose() => _controller.dispose();

  @override
  String get debugLabel => 'useCodeController';
}
