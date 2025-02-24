from flask import Flask, render_template, request, redirect, url_for
import os
from LexicalAnalysis import lex
from markupsafe import Markup
import webbrowser
from threading import Timer, Thread
import concurrent.futures
import time 
from concurrent.futures import ProcessPoolExecutor

app = Flask(__name__)

ALLOWED_EXTENSIONS = {'py', 'java', 'sql'}
FILE_DIR = 'files'  


def allowed_file(filename):
    return '.' in filename and filename.rsplit('.', 1)[1].lower() in ALLOWED_EXTENSIONS

def process_file(file_path):
    try:
        with open(file_path, 'r') as file:
            content = file.read()
        return lex(content)
    except Exception as e:
        return f"Error processing {file_path}: {e}"

def process_file_with_timing(file_path):
    start_time = time.time()  
    try:
        with open(file_path, 'r') as file:
            content = file.read()
        result = lex(content)  
    except Exception as e:
        result = f"Error procesando {file_path}: {e}"
    elapsed_time = time.time() - start_time
    return result, elapsed_time


def process_files_in_parallel(file_paths):
    results = {} 
    with ProcessPoolExecutor(max_workers=1) as executor:
        future_to_file = {executor.submit(process_file_with_timing, path): path for path in file_paths}

        for future in concurrent.futures.as_completed(future_to_file):
            file_path = future_to_file[future]
            try:
                result, elapsed_time = future.result()  
                results[file_path] = {
                    "content": result,
                    "time": elapsed_time,
                }
            except Exception as exc:
                results[file_path] = {
                    "content": f"Error procesando {file_path}: {exc}",
                    "time": None,
                }
    return results


@app.route('/', methods=['GET', 'POST'])
def index():
    if request.method == 'POST':
        filename = request.form.get('filename')
        if filename and allowed_file(filename):
            return redirect(url_for('show_file', filename=filename))
        else:
            return "Archivo no válido", 400

    files = [f for f in os.listdir(FILE_DIR) if allowed_file(f)]
    return render_template('index.html', files=files)

@app.route('/show_file', methods=['GET'])
def show_file():
    filename = request.args.get('filename') 
    if not filename or not allowed_file(filename):
        return "Archivo no válido", 400
    
    file_path = os.path.join(FILE_DIR, filename)

    if not os.path.exists(file_path):
        return "Archivo no encontrado", 404

    highlighted_content = process_file(file_path)
    files = [f for f in os.listdir(FILE_DIR) if allowed_file(f)]
    return render_template('show_file.html', content=Markup(highlighted_content), filename=filename, files=files)


@app.route('/process_sequential', methods=['GET'])
def process_sequential():
    # Obtener la lista de archivos permitidos en la carpeta FILE_DIR
    file_paths = [os.path.join(FILE_DIR, f) for f in os.listdir(FILE_DIR) if allowed_file(f)]
    
    results = {}
    total_time = 0

    for file_path in file_paths:
        start_time = time.time()
        try:
            result = process_file(file_path)  # Analizar el archivo
            elapsed_time = time.time() - start_time
            results[file_path] = {
                "content": result,
                "time": elapsed_time,
            }
            total_time += elapsed_time
        except Exception as exc:
            results[file_path] = {
                "content": f"Error procesando {file_path}: {exc}",
                "time": None,
            }

    return render_template('seq_results.html', results=results, total_time=total_time)


@app.route('/process_all', methods=['GET'])
def process_all():
    file_paths = [os.path.join(FILE_DIR, f) for f in os.listdir(FILE_DIR) if allowed_file(f)]

    results = process_files_in_parallel(file_paths)

    # Calcular el tiempo total
    total_time = sum(result["time"] for result in results.values() if result["time"] is not None)

    return render_template('results.html', results=results, total_time=total_time)





def open_browser():
    webbrowser.open_new("http://127.0.0.1:5000/")

if __name__ == '__main__':
    if os.environ.get("WERKZEUG_RUN_MAIN") == "true":
        Timer(1, open_browser).start()
    app.run(debug=True)
