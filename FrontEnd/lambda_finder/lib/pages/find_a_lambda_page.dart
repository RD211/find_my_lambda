// ignore_for_file: depend_on_referenced_packages, prefer_function_declarations_over_variables, library_private_types_in_public_api

import 'dart:math';

import 'package:awesome_snackbar_content/awesome_snackbar_content.dart';
import 'package:flutter/material.dart';
import 'package:flutter_hooks/flutter_hooks.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:lambda_finder/models/search_payload.dart';
import 'package:lambda_finder/pages/view_a_lambda_page.dart';
import 'package:lambda_finder/service/lambda_service.dart';
import 'package:tuple/tuple.dart';

import '../graph_viewer/GraphView.dart';
import '../models/lambda.dart';
import '../widgets/add_remove_input_output_widget.dart';
import '../widgets/input_output_widget.dart';

class FindALambdaPage extends StatefulHookConsumerWidget {
  const FindALambdaPage({Key? key}) : super(key: key);

  @override
  _FindALambdaPageState createState() => _FindALambdaPageState();
}

class _FindALambdaPageState extends ConsumerState<FindALambdaPage> {
  Random r = Random();

  Widget lambdaWidget(Lambda lambda) {
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
              BoxShadow(color: Colors.blue[100]!, spreadRadius: 1),
            ],
          ),
          child: Column(children: [
            Text(lambda.name),
            Text(lambda.programmingLanguage),
          ])),
    );
  }

  Graph fromLambdasToGraph(List<Lambda>? lambdas) {
    final Graph g = Graph();
    if (lambdas == null) return g;
    final nodes = lambdas.asMap().entries.map((e) => Node.Id(e)).toList();

    g.addNodes(nodes);

    for (int i = 0; i < nodes.length - 1; i++) {
      g.addEdge(nodes[i], nodes[i + 1], paint: Paint()..color = Colors.blue);
    }

    return g;
  }

  Widget graphView = const SizedBox.shrink();
  @override
  Widget build(BuildContext context) {
    final numberOfControllers = useState(1);
    final inputControllers = useState(
      List<Tuple2>.generate(
        100,
        (index) => Tuple2<TextEditingController, TextEditingController>(
          useTextEditingController(),
          useTextEditingController(),
        ),
      ),
    );

    final foundLambda = useState<List<Lambda>?>(null);

    final searchLambdas = () async {
      var inputs = <String>[];
      var results = <String>[];

      for (int i = 0; i < numberOfControllers.value; i++) {
        inputs.add(
            (inputControllers.value[i].item1 as TextEditingController).text);
        results.add(
            (inputControllers.value[i].item2 as TextEditingController).text);
      }

      var search = SearchPayload(inputs: inputs, results: results);

      graphView = const SizedBox.shrink();
      setState(() {});

      final res = ref.read(lambdaServiceProvider).searchLambda(search);

      var waitSnack = SnackBar(
        elevation: 0,
        behavior: SnackBarBehavior.floating,
        backgroundColor: Colors.transparent,
        content: AwesomeSnackbarContent(
          title: 'Search sent!',
          message: 'We are now waiting on the server. Be patient!',
          contentType: ContentType.warning,
        ),
      );

      ScaffoldMessenger.of(context).showSnackBar(waitSnack);

      res.catchError((error, stackTrace) {
        var snackBar = SnackBar(
          elevation: 0,
          behavior: SnackBarBehavior.floating,
          backgroundColor: Colors.transparent,
          content: AwesomeSnackbarContent(
            title: 'Failed to find lambda!',
            message:
                'Something went wrong while searching for the lambdas. Try again with different inputs.',
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
            title: 'Found lambdas!',
            message: 'We have found the lambdas for you check them out!',
            contentType: ContentType.success,
          ),
        );

        graphView = GraphView(
          graph: fromLambdasToGraph(value),
          algorithm: FruchtermanReingoldAlgorithm(),
          animated: true,
          paint: Paint()
            ..color = Colors.green
            ..strokeWidth = 1
            ..style = PaintingStyle.stroke,
          builder: (Node node) {
            var a = node.key!.value as MapEntry<int, Lambda>;
            return lambdaWidget(a.value);
          },
        );

        setState(() {});

        foundLambda.value = value;

        ScaffoldMessenger.of(context).showSnackBar(snackBar);
      });
    };

    return Scaffold(
        body: Row(
      children: [
        Expanded(
          flex: 2,
          child: Padding(
            padding: const EdgeInsets.all(16.0),
            child: Column(
              children: [
                Expanded(
                  child: ListView.builder(
                    primary: false,
                    itemBuilder: (context, index) {
                      if (index < numberOfControllers.value) {
                        return InputOutputWidget(
                          inputController: inputControllers.value[index].item1,
                          outputController: inputControllers.value[index].item2,
                        );
                      }
                      return AddRemoveInputsWidget(
                          numberOfControllers: numberOfControllers);
                    },
                    itemCount: numberOfControllers.value + 1,
                  ),
                ),
                TextButton(
                  style: TextButton.styleFrom(
                    backgroundColor: Colors.green,
                  ),
                  onPressed: searchLambdas,
                  child: const Text(
                    "Search for lambdas!",
                    style: TextStyle(color: Colors.white, fontSize: 24),
                  ),
                ),
              ],
            ),
          ),
        ),
        Expanded(
          flex: 3,
          child: Container(
              color: Theme.of(context).colorScheme.secondary, child: graphView),
        ),
      ],
    ));
  }
}
