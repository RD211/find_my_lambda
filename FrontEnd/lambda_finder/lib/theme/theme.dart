import 'package:flutter/material.dart';
import 'package:lambda_finder/theme/text_themes.dart';
import './elevated_button_theme.dart';
import './outlined_button_theme.dart';
import './text_button_theme.dart';

final themeData = ThemeData(
  colorScheme: ColorScheme.fromSwatch(primarySwatch: Colors.blue)
      .copyWith(secondary: Colors.purple),
  textButtonTheme: textButtonTheme,
  elevatedButtonTheme: elevatedButtonTheme,
  outlinedButtonTheme: outlinedButtonTheme,
  textTheme: textThemes,
);
