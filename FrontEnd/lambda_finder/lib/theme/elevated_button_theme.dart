import 'package:flutter/material.dart';

final ElevatedButtonThemeData elevatedButtonTheme = ElevatedButtonThemeData(
  style: ElevatedButton.styleFrom(
    padding: const EdgeInsets.symmetric(horizontal: 30, vertical: 10),
    shape: RoundedRectangleBorder(
      borderRadius: BorderRadius.circular(10),
    ),
    primary: Colors.blue, // background color
    elevation: 10,
  ),
);
