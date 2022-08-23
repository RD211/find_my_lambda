import 'package:flutter/material.dart';
import 'package:desktop/desktop.dart' as desktop;

import 'lambda_text_field_input_widget.dart';

class LambdaFormWidget extends StatelessWidget {
  const LambdaFormWidget({
    Key? key,
    required this.nameController,
    required this.descriptionController,
    required this.emailController,
    this.sendLambda,
  }) : super(key: key);

  final TextEditingController nameController;
  final TextEditingController descriptionController;
  final TextEditingController emailController;
  final Function()? sendLambda;

  @override
  Widget build(BuildContext context) {
    final readOnly = sendLambda == null;
    return Stack(
      children: [
        ListView(
          primary: false,
          children: [
            Column(
              children: [
                desktop.Text(
                  readOnly ? "View this lambda" : "Upload a lambda!",
                  style: const TextStyle(
                    fontSize: 36,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                SizedBox.fromSize(
                  size: const Size.fromHeight(25),
                ),
                LambdaTextFieldInput(
                  text: "Name ",
                  nameController: nameController,
                  readOnly: readOnly,
                ),
                SizedBox.fromSize(
                  size: const Size.fromHeight(25),
                ),
                LambdaTextFieldInput(
                  text: "Description ",
                  nameController: descriptionController,
                  maxLines: 15,
                  readOnly: readOnly,
                ),
                SizedBox.fromSize(
                  size: const Size.fromHeight(25),
                ),
                LambdaTextFieldInput(
                  text: "Email ",
                  nameController: emailController,
                  readOnly: readOnly,
                ),
              ],
            ),
          ],
        ),
        Positioned(
          bottom: 0,
          right: 0,
          child: readOnly
              ? TextButton(
                  style: TextButton.styleFrom(
                    backgroundColor: Colors.green,
                  ),
                  onPressed: () {
                    Navigator.of(context).pop();
                  },
                  child: const Text(
                    "Go back!",
                    style: TextStyle(color: Colors.white, fontSize: 24),
                  ),
                )
              : TextButton(
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
