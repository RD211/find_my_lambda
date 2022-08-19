import 'package:flutter/material.dart';
import 'package:desktop/desktop.dart' as desktop;

class AddLambdaTextFieldInput extends StatelessWidget {
  const AddLambdaTextFieldInput({
    Key? key,
    required this.nameController,
    required this.text,
    this.maxLines,
  }) : super(key: key);

  final TextEditingController nameController;
  final String text;
  final int? maxLines;

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        desktop.Text(
          text,
          style: const TextStyle(
            fontSize: 16,
          ),
        ),
        desktop.TextField(
          decoration: BoxDecoration(
            color: Theme.of(context).colorScheme.secondary,
          ),
          style: const TextStyle(
            color: Colors.white,
            fontSize: 20,
          ),
          controller: nameController,
          maxLines: maxLines ?? 1,
        ),
      ],
    );
  }
}
