import 'package:desktop/desktop.dart' as desktop;
import 'package:flutter/material.dart';
import 'package:flutter_hooks/flutter_hooks.dart';
import 'package:lambda_finder/pages/add_a_lambda_page.dart';

class HomePage extends HookWidget {
  const HomePage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body: desktop.Nav(trailingMenu: [
      desktop.NavItem(
        title: 'settings',
        builder: (context, i) => Container(
          alignment: Alignment.center,
          padding: const EdgeInsets.all(32.0),
          width: 600.0,
          child: const Text(
            'Settings page',
          ),
        ),
        icon: desktop.Icons.settings,
      ),
    ], items: [
      desktop.NavItem(
        builder: (context, i) => const AddALambdaPage(),
        title: 'page 0',
        icon: desktop.Icons.add,
      ),
      desktop.NavItem(
        builder: (context, i) => const AddALambdaPage(),
        title: 'page 1',
        icon: desktop.Icons.search,
      ),
    ]));
  }
}
