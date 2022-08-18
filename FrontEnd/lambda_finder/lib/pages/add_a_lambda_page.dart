import 'package:code_text_field/code_text_field.dart';
import 'package:flutter/material.dart';
import 'package:flutter_hooks/flutter_hooks.dart';
import 'package:lambda_finder/hooks/code_controller_hook.dart';
// ignore: depend_on_referenced_packages
import 'package:highlight/languages/cs.dart';
// ignore: depend_on_referenced_packages
import 'package:flutter_highlight/themes/monokai-sublime.dart';

class AddALambdaPage extends HookWidget {
  const AddALambdaPage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final codeController =
        useCodeController(language: cs, theme: monokaiSublimeTheme);
    return Scaffold(
        body: Row(
      children: [
        Expanded(
          child: Container(
            color: const Color(0xff23241f),
            child: ListView(
              children: [
                CodeField(
                  controller: codeController,
                  textStyle: const TextStyle(fontFamily: 'SourceCode'),
                ),
              ],
            ),
          ),
        ),
        Expanded(
            child: Column(
          children: const [
            Text(
              "Settings here",
            )
          ],
        )),
      ],
    ));
  }
}
