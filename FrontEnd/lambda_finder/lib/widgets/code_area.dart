// ignore_for_file: depend_on_referenced_packages, implementation_imports

import 'package:code_text_field/code_text_field.dart';
import 'package:flutter/material.dart';
import 'package:highlight/src/mode.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:desktop/desktop.dart' as desktop;
import 'package:tuple/tuple.dart';

class CodeArea extends StatelessWidget {
  const CodeArea({
    Key? key,
    required this.codeController,
    required this.language,
    required this.languages,
  }) : super(key: key);

  final AutoDisposeStateProvider<CodeController> codeController;
  final StateProvider<int> language;
  final Map<int, Tuple2<String, Mode>> languages;

  @override
  Widget build(BuildContext context) {
    return HookConsumer(
      builder: ((context, ref, child) {
        return Stack(
          children: [
            Container(
              color: Theme.of(context).colorScheme.secondary,
              child: ListView(
                children: [
                  CodeField(
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
                value: ref.watch(language),
                onSelected: (value) =>
                    ref.read(language.notifier).state = value as int,
                itemBuilder: (ctx) => languages.entries
                    .map((e) => desktop.ContextMenuItem(
                          value: e.key,
                          child: Text(e.value.item1),
                        ))
                    .toList(),
              ),
            ),
          ],
        );
      }),
    );
  }
}
