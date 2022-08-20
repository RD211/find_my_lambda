// ignore_for_file: depend_on_referenced_packages

import 'package:highlight/languages/cs.dart';
import 'package:highlight/languages/dart.dart';
import 'package:highlight/languages/python.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:lambda_finder/misc/templates/cs.dart';
import 'package:tuple/tuple.dart';

final languages = Provider(
  (ref) => {
    0: Tuple3("C#", cs, ref.read(csTemplateProvider)),
    1: Tuple3("Dart", dart, ref.read(csTemplateProvider)),
    2: Tuple3("Python", python, ref.read(csTemplateProvider))
  },
);
