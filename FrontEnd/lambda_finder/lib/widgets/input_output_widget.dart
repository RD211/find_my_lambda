import 'package:flutter/material.dart';

import 'lambda_text_field_input_widget.dart';

class InputOutputWidget extends StatelessWidget {
  const InputOutputWidget({
    Key? key,
    required this.inputController,
    required this.outputController,
  }) : super(key: key);

  final TextEditingController inputController;
  final TextEditingController outputController;

  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        Expanded(
          child: Padding(
            padding: const EdgeInsets.all(8.0),
            child: LambdaTextFieldInput(
              nameController: inputController,
              maxLines: 1,
              text: "Input",
            ),
          ),
        ),
        Expanded(
          child: Padding(
            padding: const EdgeInsets.all(8.0),
            child: LambdaTextFieldInput(
              nameController: outputController,
              maxLines: 1,
              text: "Output",
            ),
          ),
        ),
      ],
    );
  }
}
