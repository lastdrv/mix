# разбитие servu лога по файлам-сессиям
# todo: отлавливать перезапуск серву, иначе глючит

#$stderr = File.open('err.txt', 'a')

File.open('1.txt').readlines.each{ 
  |i|    
  File.open(i[/\(\d+\)/] + '.txt', 'a'){ 
    |file| 
    file.write i
  }
}
