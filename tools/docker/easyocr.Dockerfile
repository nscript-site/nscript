FROM pytorch/pytorch:1.10.0-cuda11.3-cudnn8-runtime
WORKDIR /var/
ENV EASYOCR_MODULE_PATH="/var/easyocr_data/"
COPY app/requirements.txt /var/requirements.txt
RUN python -m pip install -r requirements.txt -i https://pypi.tuna.tsinghua.edu.cn/simple