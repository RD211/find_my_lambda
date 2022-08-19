import 'package:flutter/material.dart';
import 'package:desktop/desktop.dart' as desktop;

import 'add_lambda_text_field_input_widget.dart';

class UploadLambdaFormWidget extends StatelessWidget {
  const UploadLambdaFormWidget({
    Key? key,
    required this.nameController,
    required this.descriptionController,
    required this.emailController,
    required this.sendLambda,
  }) : super(key: key);

  final TextEditingController nameController;
  final TextEditingController descriptionController;
  final TextEditingController emailController;
  final Function() sendLambda;

  @override
  Widget build(BuildContext context) {
    return Stack(
      children: [
        ListView(
          children: [
            Column(
              children: [
                const desktop.Text(
                  "Upload a lambda!",
                  style: TextStyle(
                    fontSize: 36,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                SizedBox.fromSize(
                  size: const Size.fromHeight(75),
                ),
                AddLambdaTextFieldInput(
                  text: "Name ",
                  nameController: nameController,
                ),
                SizedBox.fromSize(
                  size: const Size.fromHeight(25),
                ),
                AddLambdaTextFieldInput(
                  text: "Description ",
                  nameController: descriptionController,
                  maxLines: 15,
                ),
                SizedBox.fromSize(
                  size: const Size.fromHeight(25),
                ),
                AddLambdaTextFieldInput(
                  text: "Email ",
                  nameController: emailController,
                ),
              ],
            ),
          ],
        ),
        Positioned(
          bottom: 0,
          right: 0,
          child: TextButton(
            style: TextButton.styleFrom(
              backgroundColor: Colors.green,
            ),
            onPressed: sendLambda,
            child: const Text(
              "Send lambda!",
              style: TextStyle(color: Colors.white, fontSize: 24),
            ),
          ),
        )
      ],
    );
  }
}
