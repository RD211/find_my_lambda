// ignore_for_file: depend_on_referenced_packages, prefer_function_declarations_over_variables

import 'package:code_text_field/code_text_field.dart';
import 'package:flutter/material.dart';
import 'package:flutter_hooks/flutter_hooks.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:flutter_highlight/themes/monokai-sublime.dart';
import 'package:lambda_finder/service/lambda_service.dart';
import '../misc/language_settings.dart';
import '../models/lambda.dart';
import '../widgets/code_area.dart';
import '../widgets/lambda_form.dart';
import 'package:desktop/desktop.dart' as desktop;

class ViewALambdaPage extends HookConsumerWidget {
  final int lambdaId;
  const ViewALambdaPage({Key? key, required this.lambdaId}) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final data = ref.watch(lambdaServiceProvider).getLambda(lambdaId);
    return FutureBuilder<Lambda>(
      builder: (context, snapshot) {
        if (snapshot.hasData) {
          return ViewALambda(lambda: snapshot.data!);
        } else if (snapshot.hasError) {
          return const Text("Something went wrong or lambda does not exist.");
        } else {
          return Center(
              child: Container(
                  width: 100,
                  height: 100,
                  child: const desktop.CircularProgressIndicator()));
        }
      },
      future: data,
    );
  }
}

class ViewALambda extends HookConsumerWidget {
  final Lambda lambda;
  const ViewALambda({Key? key, required this.lambda}) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final languageInfo = ref.watch(languages).entries.firstWhere(
        (element) => element.value.item1 == lambda.programmingLanguage);

    final codeController = StateProvider.autoDispose<CodeController>((ref) {
      final controller = CodeController(
        language: languageInfo.value.item2,
        theme: monokaiSublimeTheme,
        text: lambda.code,
      );

      ref.onDispose(() {
        controller.dispose();
      });

      return controller;
    });

    final nameController = useTextEditingController.fromValue(
      TextEditingValue(
        text: lambda.name,
      ),
    );

    final descriptionController = useTextEditingController.fromValue(
      TextEditingValue(
        text: lambda.description,
      ),
    );

    final emailController = useTextEditingController.fromValue(
      TextEditingValue(
        text: lambda.email,
      ),
    );

    return Scaffold(
        body: Row(
      children: [
        Expanded(
          flex: 3,
          child: CodeArea(
            codeController: codeController,
            language: StateProvider((_) => languageInfo.key),
            readOnly: true,
          ),
        ),
        Expanded(
          flex: 2,
          child: Padding(
            padding: const EdgeInsets.all(16.0),
            child: LambdaFormWidget(
              nameController: nameController,
              descriptionController: descriptionController,
              emailController: emailController,
            ),
          ),
        ),
      ],
    ));
  }
}
