def calculate_average(grades):
    #funcion 1
    if not grades:
        return 0
    return sum(grades) / len(grades)


def determine_pass_status(average, passing_score=70):
    #funcion 2
    return average >= passing_score


def main():
    num_grades = int(input("¿Cuántas calificaciones se van a calcular? "))
    
    grades = []
    for i in range(num_grades):
        grade = int(input(f"Agregue una calificacion {i + 1}: "))
        grades.append(grade)
    
    average = calculate_average(grades)
    print(f"\nEl promedio es: {average:.2f}")
    
    if determine_pass_status(average):
        print("Pasa la materia")
    else:
        print("No pasa la materia")


if __name__ == "__main__":
    main()
