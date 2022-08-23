// ignore_for_file: depend_on_referenced_packages, implementation_imports

import 'package:code_text_field/code_text_field.dart';
import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:desktop/desktop.dart' as desktop;
import 'package:lambda_finder/misc/language_settings.dart';

class CodeArea extends HookConsumerWidget {
  const CodeArea({
    Key? key,
    required this.codeController,
    required this.language,
    this.readOnly = false,
  }) : super(key: key);

  final AutoDisposeStateProvider<CodeController> codeController;
  final StateProvider<int> language;
  final bool readOnly;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return Stack(
      children: [
        Container(
          color: Theme.of(context).colorScheme.secondary,
          child: ListView(
            primary: false,
            children: [
              CodeField(
                enabled: !readOnly,
                controller: ref.watch(codeController),
                textStyle: const TextStyle(
                  fontFamily: 'SourceCode',
                ),
              ),
            ],
          ),
        ),
        Container(
          alignment: desktop.Alignment.bottomCenter,
          child: desktop.DropDownButton(
            enabled: !readOnly,
            value: ref.read(language),
            onSelected: (value) =>
                ref.read(language.notifier).state = value as int,
            itemBuilder: (ctx) => ref
                .read(languages)
                .entries
                .map((e) => desktop.ContextMenuItem(
                      value: e.key,
                      child: Text(e.value.item1),
                    ))
                .toList(),
          ),
        ),
      ],
    );
  }
}
