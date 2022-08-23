import 'package:flutter/material.dart';
import 'package:desktop/desktop.dart' as desktop;

class AddRemoveInputsWidget extends StatelessWidget {
  const AddRemoveInputsWidget({
    Key? key,
    required this.numberOfControllers,
  }) : super(key: key);

  final ValueNotifier<int> numberOfControllers;

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        if (numberOfControllers.value < 100)
          desktop.Button(
            leading: const Icon(
              Icons.add,
              color: Colors.green,
            ),
            trailing: const Text(
              "Add another input!",
              style: TextStyle(color: Colors.green, fontSize: 16),
            ),
            onPressed: () {
              numberOfControllers.value = numberOfControllers.value + 1;
            },
            color: Colors.green,
          ),
        if (numberOfControllers.value > 1)
          desktop.Button(
            leading: const Icon(
              Icons.remove,
              color: Colors.red,
            ),
            trailing: const Text(
              "Remove input!",
              style: TextStyle(color: Colors.red, fontSize: 16),
            ),
            onPressed: () {
              numberOfControllers.value = numberOfControllers.value - 1;
            },
            color: Colors.green,
          ),
      ],
    );
  }
}
