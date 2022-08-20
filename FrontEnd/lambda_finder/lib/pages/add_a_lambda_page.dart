// ignore_for_file: depend_on_referenced_packages, prefer_function_declarations_over_variables

import 'package:awesome_snackbar_content/awesome_snackbar_content.dart';
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

class AddALambdaPage extends HookConsumerWidget {
  const AddALambdaPage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final language = StateProvider<int>((ref) => 0);
    final codeController = StateProvider.autoDispose<CodeController>((ref) {
      final int languageId = ref.watch(language);
      final languageInfo = ref.watch(languages)[languageId]!;

      final controller = CodeController(
        language: languageInfo.item2,
        theme: monokaiSublimeTheme,
        text: languageInfo.item3,
      );

      ref.onDispose(() {
        controller.dispose();
      });

      return controller;
    });

    final nameController = useTextEditingController();
    final descriptionController = useTextEditingController();
    final emailController = useTextEditingController();

    final sendLambda = () async {
      final res = ref.read(lambdaServiceProvider).postLambda(
            Lambda(
                name: nameController.text,
                description: descriptionController.text,
                email: emailController.text,
                programmingLanguage:
                    ref.read(languages)[ref.read(language)]!.item1,
                code: ref.read(codeController).rawText),
          );

      res.catchError((error, stackTrace) {
        var snackBar = SnackBar(
          elevation: 0,
          behavior: SnackBarBehavior.floating,
          backgroundColor: Colors.transparent,
          content: AwesomeSnackbarContent(
            title: 'Failed to upload!',
            message:
                'Something went wrong while uploading the lambda. Check it again please.',
            contentType: ContentType.failure,
          ),
        );

        ScaffoldMessenger.of(context).showSnackBar(snackBar);
      });

      res.then((value) {
        var snackBar = SnackBar(
          elevation: 0,
          behavior: SnackBarBehavior.floating,
          backgroundColor: Colors.transparent,
          content: AwesomeSnackbarContent(
            title: 'Lambda uploaded!',
            message: 'You have succesfully uploaded your lambda! Good job!',
            contentType: ContentType.success,
          ),
        );

        ScaffoldMessenger.of(context).showSnackBar(snackBar);
      });
    };

    return Scaffold(
        body: Row(
      children: [
        Expanded(
          flex: 3,
          child: CodeArea(
            codeController: codeController,
            language: language,
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
              sendLambda: sendLambda,
            ),
          ),
        ),
      ],
    ));
  }
}
