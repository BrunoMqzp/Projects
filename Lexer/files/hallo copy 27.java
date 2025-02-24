import java.util.Scanner;

public class GradeCalculator {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);

        System.out.println("Calculadora de promedios");

        // Prompt the user for three grades
        System.out.print("1ª cal: ");
        int grade1 = scanner.nextInt();

        System.out.print("2: ");
        int grade2 = scanner.nextInt();

        System.out.print("3: ");
        int grade3 = scanner.nextInt();

        // Calculate the average
        double average = (grade1 + grade2 + grade3) / 3.0;

        // Display the average
        System.out.printf("El promedio es: %.2f%n", average);

        // Determine pass or fail
        if (average >= 70) {
            System.out.println("Pasa la materia");
        } else {
            System.out.println("No pasa la materia");
        }

        scanner.close();
    }
}
