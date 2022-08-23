import 'package:flutter/material.dart';

import '../models/lambda.dart';
import '../pages/view_a_lambda_page.dart';

class LambdaNodeWidget extends StatelessWidget {
  const LambdaNodeWidget({Key? key, required this.lambda}) : super(key: key);

  final Lambda lambda;

  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: () {
        Navigator.push(
            context,
            MaterialPageRoute(
                builder: (_) => ViewALambdaPage(lambdaId: lambda.id!)));
      },
      child: Container(
        padding: const EdgeInsets.all(16),
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(4),
          boxShadow: [
            BoxShadow(
              color: Colors.blue[100]!,
              spreadRadius: 1,
            ),
          ],
        ),
        child: Column(
          children: [
            Text(lambda.name),
            Text(lambda.programmingLanguage),
          ],
        ),
      ),
    );
  }
}
